using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class Audit : BaseEntity
{
    public string AuditId { get; set; } = string.Empty;
    public string? AuditType { get; set; }
    public string? AuditResult { get; set; }
    public string? UserCode { get; set; }
    public string? LoginCode { get; set; }
    public string? UserName { get; set; }
    public string? OfficeCode { get; set; }
    public string? OfficeName { get; set; }
    public string? MenuCode { get; set; }
    public string? PwdSecurityLevel { get; set; }
    public DateTime? PwdUpdateDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public DateTime? EndDate { get; set; }
}
