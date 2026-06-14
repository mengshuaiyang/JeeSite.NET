using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

/// <summary>
/// 岗位实体，代表公司内部岗位/职级（如：产品经理、高级工程师）。员工通过分配岗位获得职级与权限标签。
/// </summary>
public class Post : DataEntity, ICorpEntity
{
    /// <summary>岗位编码，业务主键。</summary>
    public string PostCode { get; set; } = string.Empty;
    /// <summary>岗位名称。</summary>
    public string PostName { get; set; } = string.Empty;
    /// <summary>视图编码。</summary>
    public string? ViewCode { get; set; }
    /// <summary>岗位类型：management（管理岗）/ professional（专业岗）/ operation（操作岗）。</summary>
    public string? PostType { get; set; }
    /// <summary>岗位排序（数值越小越靠前）。</summary>
    public decimal? PostSort { get; set; }
    /// <summary>所属机构编码（引用 Organization.OrgCode）。</summary>
    public string? OrgCode { get; set; }

    /// <summary>公司编码（多租户/多公司隔离字段）。</summary>
    public string? CorpCode { get; set; }
    /// <summary>公司名称，冗余便于展示。</summary>
    public string? CorpName { get; set; }
}
