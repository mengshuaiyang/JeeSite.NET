using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

/// <summary>
/// 员工实体，代表公司下的职员。员工是人事档案主体，通过 EmpCode 与 User 进行一对一关联，
/// 归属到具体的机构（Office）与公司（Company）下。
/// </summary>
public class Employee : DataEntity, ICorpEntity, IExtendEntity
{
    /// <summary>员工编码，业务主键（通常与 User.UserCode 对齐）。</summary>
    public string EmpCode { get; set; } = string.Empty;
    /// <summary>工号（企业内部编号）。</summary>
    public string? EmpNo { get; set; }
    /// <summary>员工姓名。</summary>
    public string EmpName { get; set; } = string.Empty;
    /// <summary>英文名/拼音名。</summary>
    public string? EmpNameEn { get; set; }
    /// <summary>所属机构编码（引用 Organization.OrgCode）。</summary>
    public string OfficeCode { get; set; } = string.Empty;
    /// <summary>所属机构名称，冗余便于展示。</summary>
    public string OfficeName { get; set; } = string.Empty;
    /// <summary>所属公司编码（引用 Company.CompanyCode）。</summary>
    public string CompanyCode { get; set; } = string.Empty;
    /// <summary>所属公司名称，冗余便于展示。</summary>
    public string CompanyName { get; set; } = string.Empty;

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
}
