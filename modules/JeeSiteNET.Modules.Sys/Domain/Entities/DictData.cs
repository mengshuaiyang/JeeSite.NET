using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

/// <summary>
/// 字典数据实体，代表字典类型下的具体选项（如：男/女、是/否）。树形结构支持多级字典。
/// </summary>
public class DictData : TreeEntity, ICorpEntity, IExtendEntity
{
    /// <summary>字典编码，业务主键。</summary>
    public string DictCode { get; set; } = string.Empty;
    /// <summary>字典类型（引用 DictType.DictTypeCode）。</summary>
    public string DictType { get; set; } = string.Empty;
    /// <summary>字典标签/显示文本（如：男）。</summary>
    public string DictLabel { get; set; } = string.Empty;
    /// <summary>字典值/存储值（如：M）。</summary>
    public string DictValue { get; set; } = string.Empty;
    /// <summary>字典图标。</summary>
    public string? DictIcon { get; set; }
    /// <summary>字典项描述说明。</summary>
    public string? Description { get; set; }
    /// <summary>CSS 内联样式（用于列表渲染）。</summary>
    public string? CssStyle { get; set; }
    /// <summary>CSS 类名（用于列表渲染）。</summary>
    public string? CssClass { get; set; }
    /// <summary>字典项排序（数值越小越靠前）。</summary>
    public decimal? Sort { get; set; }

    /// <summary>公司编码（多租户/多公司隔离字段）。</summary>
    public string? CorpCode { get; set; }
    /// <summary>公司名称，冗余便于展示。</summary>
    public string? CorpName { get; set; }

    /// <summary>获取字典标签（用于 TreeEntity 展示）。</summary>
    public override string GetName() => DictLabel;

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
