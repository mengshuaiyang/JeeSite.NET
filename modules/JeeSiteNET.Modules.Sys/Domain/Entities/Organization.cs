using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

/// <summary>
/// 组织机构实体，树形结构，代表公司下属部门/处室。员工通过 OfficeCode 归属到具体机构下。
/// </summary>
public class Organization : TreeEntity, ICorpEntity, IExtendEntity
{
    /// <summary>机构编码，业务主键。</summary>
    public string OrgCode { get; set; } = string.Empty;
    /// <summary>机构名称。</summary>
    public string OrgName { get; set; } = string.Empty;
    /// <summary>视图编码，用于多视图分类（如：行政视图、财务视图）。</summary>
    public string? ViewCode { get; set; }
    /// <summary>机构全称。</summary>
    public string? FullName { get; set; }
    /// <summary>机构类型：1=公司、2=部门、3=小组、4=岗位。</summary>
    public string? OrgType { get; set; }
    /// <summary>机构负责人（UserCode 或姓名）。</summary>
    public string? Leader { get; set; }
    /// <summary>联系电话。</summary>
    public string? Phone { get; set; }
    /// <summary>办公地址。</summary>
    public string? Address { get; set; }
    /// <summary>邮政编码。</summary>
    public string? ZipCode { get; set; }
    /// <summary>联系邮箱。</summary>
    public string? Email { get; set; }

    /// <summary>公司编码（多租户/多公司隔离字段）。</summary>
    public string? CorpCode { get; set; }
    /// <summary>公司名称，冗余便于展示。</summary>
    public string? CorpName { get; set; }

    /// <summary>扩展字符串字段 1。</summary>
    public string? ExtendS1 { get; set; }
    /// <summary>扩展字符串字段 2。</summary>
    public string? ExtendS2 { get; set; }
    /// <summary>扩展字符串字段 3。</summary>
    public string? ExtendS3 { get; set; }
    /// <summary>扩展字符串字段 4。</summary>
    public string? ExtendS4 { get; set; }
    /// <summary>扩展字符串字段 5。</summary>
    public string? ExtendS5 { get; set; }
    /// <summary>扩展字符串字段 6。</summary>
    public string? ExtendS6 { get; set; }
    /// <summary>扩展字符串字段 7。</summary>
    public string? ExtendS7 { get; set; }
    /// <summary>扩展字符串字段 8。</summary>
    public string? ExtendS8 { get; set; }
    /// <summary>扩展整型字段 1。</summary>
    public int? ExtendI1 { get; set; }
    /// <summary>扩展整型字段 2。</summary>
    public int? ExtendI2 { get; set; }
    /// <summary>扩展整型字段 3。</summary>
    public int? ExtendI3 { get; set; }
    /// <summary>扩展整型字段 4。</summary>
    public int? ExtendI4 { get; set; }
    /// <summary>扩展十进制字段 1。</summary>
    public decimal? ExtendF1 { get; set; }
    /// <summary>扩展十进制字段 2。</summary>
    public decimal? ExtendF2 { get; set; }
    /// <summary>扩展十进制字段 3。</summary>
    public decimal? ExtendF3 { get; set; }
    /// <summary>扩展十进制字段 4。</summary>
    public decimal? ExtendF4 { get; set; }
    /// <summary>扩展日期字段 1。</summary>
    public DateTime? ExtendD1 { get; set; }
    /// <summary>扩展日期字段 2。</summary>
    public DateTime? ExtendD2 { get; set; }
    /// <summary>扩展日期字段 3。</summary>
    public DateTime? ExtendD3 { get; set; }
    /// <summary>扩展日期字段 4。</summary>
    public DateTime? ExtendD4 { get; set; }
    /// <summary>扩展 JSON 字段，用于存储自定义结构化数据。</summary>
    public string? ExtendJson { get; set; }

    /// <summary>获取机构名称（用于 TreeEntity 展示）。</summary>
    public override string GetName() => OrgName;
}
