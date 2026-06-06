using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class Tenant : DataEntity, ITenantEntity
{
    public string TenantCode { get; set; } = string.Empty;
    public string TenantName { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? ContactPhone { get; set; }
    public string? ExpireDate { get; set; }
    public string? IsAvailable { get; set; } = "1";
    string? ITenantEntity.TenantCode { get => TenantCode; set => TenantCode = value ?? string.Empty; }
}
