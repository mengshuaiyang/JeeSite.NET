namespace JeeSiteNET.Core;

public class MultiTenantDbConfig
{
    public string TenantCode { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
    public string DbProvider { get; set; } = "SqlServer";
}

public class MultiDataSourceOptions
{
    public List<MultiTenantDbConfig> Tenants { get; set; } = [];
    public string DefaultConnectionString { get; set; } = string.Empty;
    public bool EnableReadWriteSplitting { get; set; }
    public List<string> ReadConnections { get; set; } = [];
    public string WriteConnection { get; set; } = string.Empty;
}
