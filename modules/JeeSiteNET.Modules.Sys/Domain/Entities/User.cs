using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class User : DataEntity
{
    public string UserCode { get; set; } = string.Empty;
    public string LoginCode { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string UserType { get; set; } = "employee";
    public string? Avatar { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? OrgCode { get; set; }
    public string? OrgName { get; set; }
    public DateTime? LoginDate { get; set; }
    public string? LoginIp { get; set; }
    public decimal? LoginCount { get; set; }
    public DateTime? PwdUpdateDate { get; set; }
}
