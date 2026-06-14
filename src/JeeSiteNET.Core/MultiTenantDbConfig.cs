namespace JeeSiteNET.Core;

/// <summary>
/// 多租户数据库配置：映射单个租户的数据库连接
/// </summary>
public class MultiTenantDbConfig
{
    /// <summary>
    /// 租户编码（唯一标识一个租户）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 该租户的数据库连接字符串
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// 数据库提供程序（如 SqlServer / PostgreSQL / MySql 等），默认值 "SqlServer"
    /// </summary>
    public string DbProvider { get; set; } = "SqlServer";
}

/// <summary>
/// 多数据源配置：用于读写分离/横向扩展的多库场景
/// </summary>
public class DataSourceConfig
{
    /// <summary>
    /// 数据源名称（用于路由引用）
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 该数据源的连接字符串
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// 数据库提供程序（如 SqlServer / PostgreSQL 等），默认值 "SqlServer"
    /// </summary>
    public string DbProvider { get; set; } = "SqlServer";

    /// <summary>
    /// 是否主库（写操作路由到主库），默认值 true
    /// </summary>
    public bool IsMaster { get; set; } = true;
}

/// <summary>
/// 多数据源聚合配置选项：从配置文件绑定，供 DI 注册使用
/// </summary>
public class MultiDataSourceOptions
{
    /// <summary>
    /// 按租户划分的数据库配置列表（多租户场景）
    /// </summary>
    public List<MultiTenantDbConfig> Tenants { get; set; } = [];

    /// <summary>
    /// 未命中租户时使用的默认连接字符串
    /// </summary>
    public string DefaultConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// 是否启用读写分离（启用后读操作会路由到从库），默认值 false
    /// </summary>
    public bool EnableReadWriteSplitting { get; set; }

    /// <summary>
    /// 从库负载均衡算法（如 RoundRobin / LeastConnections），默认值 "RoundRobin"
    /// </summary>
    public string LoadBalancerAlgorithm { get; set; } = "RoundRobin";

    /// <summary>
    /// 多数据源列表（用于读写分离与多从库场景）
    /// </summary>
    public List<DataSourceConfig> DataSources { get; set; } = [];

    /// <summary>
    /// 写操作的主连接字符串（显式指定主库）
    /// </summary>
    public string WriteConnection { get; set; } = string.Empty;
}
