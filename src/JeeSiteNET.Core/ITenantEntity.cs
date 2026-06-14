namespace JeeSiteNET.Core;

/// <summary>
/// 多租户实体接口：标识实体需要按 TenantCode 隔离数据
/// </summary>
public interface ITenantEntity
{
    /// <summary>
    /// 租户编码（用于隔离不同租户的数据）
    /// </summary>
    string? TenantCode { get; set; }
}
