namespace JeeSiteNET.Core;

/// <summary>
/// 多租户上下文：持有当前请求的租户信息
/// </summary>
public class TenantContext : ITenantContext
{
    /// <summary>
    /// 租户编码（多租户系统用于隔离数据）
    /// </summary>
    public string? TenantCode { get; set; }

    /// <summary>
    /// 租户名称（用于日志或展示）
    /// </summary>
    public string? TenantName { get; set; }

    /// <summary>
    /// 是否启用多租户模式（TenantCode 非空即视为启用）
    /// </summary>
    public bool IsMultiTenant => !string.IsNullOrEmpty(TenantCode);
}

/// <summary>
/// 多租户上下文接口：用于 DI 注入和测试替换
/// </summary>
public interface ITenantContext
{
    /// <summary>
    /// 租户编码
    /// </summary>
    string? TenantCode { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    string? TenantName { get; set; }

    /// <summary>
    /// 是否启用多租户模式
    /// </summary>
    bool IsMultiTenant { get; }
}
