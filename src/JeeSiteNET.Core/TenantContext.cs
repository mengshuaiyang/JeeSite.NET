namespace JeeSiteNET.Core;

public class TenantContext : ITenantContext
{
    public string? TenantCode { get; set; }
    public string? TenantName { get; set; }
    public bool IsMultiTenant => !string.IsNullOrEmpty(TenantCode);
}
