namespace JeeSiteNET.Modules.Sys.Application.DTOs;

/// <summary>租户信息 DTO，用于展示和返回租户数据。</summary>
public class TenantDto
{
    /// <summary>租户编码。</summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>租户名称。</summary>
    public string TenantName { get; set; } = string.Empty;

    /// <summary>联系人姓名。</summary>
    public string? ContactName { get; set; }

    /// <summary>联系人电话。</summary>
    public string? ContactPhone { get; set; }

    /// <summary>是否可用：1=可用，0=停用。</summary>
    public string? IsAvailable { get; set; }

    /// <summary>状态（来自 DataEntity）。</summary>
    public string? Status { get; set; }
}

/// <summary>租户保存 DTO，用于新增或更新租户信息。</summary>
public class TenantSaveDto
{
    /// <summary>租户编码（为空时表示新增，自动生成）。</summary>
    public string? TenantCode { get; set; }

    /// <summary>租户名称。</summary>
    public string TenantName { get; set; } = string.Empty;

    /// <summary>联系人姓名。</summary>
    public string? ContactName { get; set; }

    /// <summary>联系人电话。</summary>
    public string? ContactPhone { get; set; }

    /// <summary>是否可用：1=可用，0=停用，默认 1。</summary>
    public string? IsAvailable { get; set; }
}
