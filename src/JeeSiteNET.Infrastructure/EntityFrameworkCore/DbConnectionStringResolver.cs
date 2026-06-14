using JeeSiteNET.Core;
using Microsoft.Extensions.Configuration;

namespace JeeSiteNET.Infrastructure.EntityFrameworkCore;

/// <summary>
/// 多数据源/多租户连接字符串解析器：支持读写分离（轮询/随机）与按租户切换连接。
/// 从 IConfiguration 的 "MultiDataSource" 节点读取配置，无配置时回退到默认连接字符串。
/// </summary>
public class DbConnectionStringResolver : IDbConnectionStringResolver
{
    /// <summary>
    /// 配置对象
    /// </summary>
    private readonly IConfiguration _configuration;
    /// <summary>
    /// 当前租户上下文（用于按租户解析目标连接）
    /// </summary>
    private readonly ITenantContext _tenantContext;
    /// <summary>
    /// 解析后的多数据源选项
    /// </summary>
    private readonly MultiDataSourceOptions _options;
    /// <summary>
    /// 读库轮询索引（通过 Interlocked 保证线程安全递增）
    /// </summary>
    private int _roundRobinIndex;

    /// <summary>
    /// 构造函数：从 IConfiguration 加载 MultiDataSource 节点
    /// </summary>
    /// <param name="configuration">应用配置</param>
    /// <param name="tenantContext">当前租户上下文（可选，由 DI 注入）</param>
    public DbConnectionStringResolver(IConfiguration configuration, ITenantContext tenantContext)
    {
        _configuration = configuration;
        _tenantContext = tenantContext;
        _options = LoadOptions();
    }

    /// <summary>
    /// 从 IConfiguration 加载 MultiDataSource 选项
    /// </summary>
    /// <returns>多数据源选项</returns>
    private MultiDataSourceOptions LoadOptions()
    {
        var opts = new MultiDataSourceOptions();
        var section = _configuration.GetSection("MultiDataSource");
        if (!section.Exists()) return opts;

        opts.EnableReadWriteSplitting = section.GetValue<bool>("EnableReadWriteSplitting");
        opts.LoadBalancerAlgorithm = section["LoadBalancerAlgorithm"] ?? "RoundRobin";
        opts.DefaultConnectionString = _configuration.GetConnectionString("DefaultConnection") ?? "";

        var dataSources = section.GetSection("DataSources").GetChildren();
        foreach (var ds in dataSources)
        {
            opts.DataSources.Add(new DataSourceConfig
            {
                Name = ds.Key,
                ConnectionString = ds["ConnectionString"] ?? "",
                DbProvider = ds["DbProvider"] ?? "SqlServer",
                IsMaster = ds.GetValue<bool>("IsMaster")
            });
        }

        var tenants = section.GetSection("Tenants").GetChildren();
        foreach (var t in tenants)
        {
            opts.Tenants.Add(new MultiTenantDbConfig
            {
                TenantCode = t.Key,
                ConnectionString = t["ConnectionString"] ?? "",
                DbProvider = t["DbProvider"] ?? "SqlServer"
            });
        }

        return opts;
    }

    /// <summary>
    /// 按租户和操作类型获取连接字符串：优先按租户解析，否则回落到读写分离逻辑
    /// </summary>
    /// <param name="tenantCode">目标租户编号（null 时使用 ITenantContext）</param>
    /// <param name="operation">操作类型：Read 走读库，Write 走主库</param>
    /// <returns>匹配的连接字符串</returns>
    public string GetConnectionString(string? tenantCode = null, DbOperation operation = DbOperation.Write)
    {
        var code = tenantCode ?? _tenantContext.TenantCode;

        if (!string.IsNullOrEmpty(code))
        {
            var tenant = _options.Tenants.Find(t => t.TenantCode == code);
            if (tenant != null && !string.IsNullOrEmpty(tenant.ConnectionString))
                return tenant.ConnectionString;
        }

        return GetConnectionString(operation);
    }

    /// <summary>
    /// 仅按操作类型解析连接字符串：支持读库负载均衡（轮询/随机）与主库回退
    /// </summary>
    /// <param name="operation">操作类型</param>
    /// <returns>连接字符串</returns>
    public string GetConnectionString(DbOperation operation)
    {
        if (_options.EnableReadWriteSplitting && operation == DbOperation.Read)
        {
            var readSources = _options.DataSources.Where(ds => !ds.IsMaster).ToList();
            if (readSources.Count > 0)
            {
                var index = _options.LoadBalancerAlgorithm == "Random"
                    ? Random.Shared.Next(readSources.Count)
                    : Interlocked.Increment(ref _roundRobinIndex) % readSources.Count;
                return readSources[Math.Abs(index % readSources.Count)].ConnectionString;
            }
        }

        var master = _options.DataSources.Find(ds => ds.IsMaster);
        if (master != null && !string.IsNullOrEmpty(master.ConnectionString))
            return master.ConnectionString;

        if (!string.IsNullOrEmpty(_options.DefaultConnectionString))
            return _options.DefaultConnectionString;

        return _configuration.GetConnectionString("DefaultConnection")
            ?? "Server=localhost;Database=JeeSiteNET;Trusted_Connection=true;TrustServerCertificate=true";
    }
}
