using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

/// <summary>
/// 审计实体，记录高敏感操作（登录、登出、权限变更、密码修改等），供安全审计与合规检查使用。
/// </summary>
public class Audit : BaseEntity
{
    /// <summary>审计记录 ID，业务主键。</summary>
    public string AuditId { get; set; } = string.Empty;
    /// <summary>审计类型：login（登录）、logout（登出）、pwd_change（改密）、perm_change（权限变更）。</summary>
    public string? AuditType { get; set; }
    /// <summary>审计结果：success（成功）、fail（失败）。</summary>
    public string? AuditResult { get; set; }
    /// <summary>操作人用户编码。</summary>
    public string? UserCode { get; set; }
    /// <summary>操作人登录账号。</summary>
    public string? LoginCode { get; set; }
    /// <summary>操作人姓名。</summary>
    public string? UserName { get; set; }
    /// <summary>操作人所属机构编码。</summary>
    public string? OfficeCode { get; set; }
    /// <summary>操作人所属机构名称。</summary>
    public string? OfficeName { get; set; }
    /// <summary>关联菜单编码，记录操作发生的功能入口。</summary>
    public string? MenuCode { get; set; }
    /// <summary>操作时用户密码安全等级快照。</summary>
    public string? PwdSecurityLevel { get; set; }
    /// <summary>密码最近一次更新时间。</summary>
    public DateTime? PwdUpdateDate { get; set; }
    /// <summary>最近一次登录时间。</summary>
    public DateTime? LastLoginDate { get; set; }
    /// <summary>审计事件结束时间（会话结束/登出时间）。</summary>
    public DateTime? EndDate { get; set; }
}
