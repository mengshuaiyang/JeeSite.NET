namespace JeeSiteNET.Modules.Sys.Application.DTOs;

/// <summary>
/// 岗位信息 DTO。
/// </summary>
public class PostDto
{
    /// <summary>
    /// 岗位编码（主键）。
    /// </summary>
    public string PostCode { get; set; } = string.Empty;

    /// <summary>
    /// 岗位名称。
    /// </summary>
    public string PostName { get; set; } = string.Empty;

    /// <summary>
    /// 所属机构编码。
    /// </summary>
    public string? OrgCode { get; set; }

    /// <summary>
    /// 排序值。
    /// </summary>
    public decimal? Sort { get; set; }

    /// <summary>
    /// 状态（0 正常 / 1 禁用）。
    /// </summary>
    public string? Status { get; set; }
}

/// <summary>
/// 岗位保存请求 DTO。
/// </summary>
public class PostSaveDto
{
    /// <summary>
    /// 岗位编码；空表示新建。
    /// </summary>
    public string? PostCode { get; set; }

    /// <summary>
    /// 岗位名称。
    /// </summary>
    public string PostName { get; set; } = string.Empty;

    /// <summary>
    /// 所属机构编码。
    /// </summary>
    public string? OrgCode { get; set; }

    /// <summary>
    /// 排序值。
    /// </summary>
    public decimal? Sort { get; set; }
}
