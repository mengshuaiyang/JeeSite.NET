using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class Role : DataEntity
{
    public string RoleCode { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public string? RoleType { get; set; }
    public string? IsSys { get; set; } = "0";
    public string? UserType { get; set; }
    public decimal? Sort { get; set; }
    public string? DataScope { get; set; }
}
