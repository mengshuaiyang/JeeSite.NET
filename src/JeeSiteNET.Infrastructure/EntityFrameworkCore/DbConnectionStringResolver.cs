using JeeSiteNET.Core;
using Microsoft.Extensions.Configuration;

namespace JeeSiteNET.Infrastructure.EntityFrameworkCore;

public class DbConnectionStringResolver : IDbConnectionStringResolver
{
    private readonly IConfiguration _configuration;
    private readonly ITenantContext _tenantContext;
    private readonly MultiDataSourceOptions _options;
    private int _roundRobinIndex;

    public DbConnectionStringResolver(IConfiguration configuration, ITenantContext tenantContext)
    {
        _configuration = configuration;
        _tenantContext = tenantContext;
        _options = LoadOptions();
    }

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
