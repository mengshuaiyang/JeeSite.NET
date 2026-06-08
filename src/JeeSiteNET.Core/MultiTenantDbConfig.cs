namespace JeeSiteNET.Core;

public class MultiTenantDbConfig
{
    public string TenantCode { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
    public string DbProvider { get; set; } = "SqlServer";
}

public class DataSourceConfig
{
    public string Name { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
    public string DbProvider { get; set; } = "SqlServer";
    public bool IsMaster { get; set; } = true;
}

public class MultiDataSourceOptions
{
    public List<MultiTenantDbConfig> Tenants { get; set; } = [];
    public string DefaultConnectionString { get; set; } = string.Empty;
    public bool EnableReadWriteSplitting { get; set; }
    public string LoadBalancerAlgorithm { get; set; } = "RoundRobin";
    public List<DataSourceConfig> DataSources { get; set; } = [];
    public string WriteConnection { get; set; } = string.Empty;
}
