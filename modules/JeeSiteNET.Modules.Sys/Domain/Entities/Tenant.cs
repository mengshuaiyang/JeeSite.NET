using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

/// <summary>
/// 租户实体，代表 SaaS 多租户场景下的一个独立客户/组织。租户下包含公司、用户、业务数据等隔离资源。
/// </summary>
public class Tenant : DataEntity, ITenantEntity
{
    /// <summary>租户编码，业务主键。</summary>
    public string TenantCode { get; set; } = string.Empty;
    /// <summary>租户名称（客户/组织名称）。</summary>
    public string TenantName { get; set; } = string.Empty;
    /// <summary>联系人姓名。</summary>
    public string? ContactName { get; set; }
    /// <summary>联系人电话。</summary>
    public string? ContactPhone { get; set; }
    /// <summary>租户到期日期（yyyy-MM-dd，到期后禁止登录）。</summary>
    public string? ExpireDate { get; set; }
    /// <summary>是否可用：1=可用，0=停用，默认 1。</summary>
    public string? IsAvailable { get; set; } = "1";
    /// <summary>显式实现 ITenantEntity，返回与 TenantCode 对齐的租户编码。</summary>
    string? ITenantEntity.TenantCode { get => TenantCode; set => TenantCode = value ?? string.Empty; }
}
