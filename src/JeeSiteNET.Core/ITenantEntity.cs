namespace JeeSiteNET.Core;

public interface ITenantEntity
{
    string? TenantCode { get; set; }
}

public interface ITenantContext
{
    string? TenantCode { get; }
    string? TenantName { get; }
    bool IsMultiTenant { get; }
}
