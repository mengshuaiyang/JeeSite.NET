using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

/// <summary>
/// 字典类型实体，代表一类字典的分组（如 sys_user_sex、sys_common_status）。
/// 具体字典项由 DictData 承载，两者通过 DictType 字段关联。
/// </summary>
public class DictType : DataEntity
{
    /// <summary>字典类型编码（字典分类 Key）。</summary>
    public string DictTypeCode { get; set; } = string.Empty;
    /// <summary>字典名称（如：用户性别、通用状态）。</summary>
    public string DictName { get; set; } = string.Empty;
    /// <summary>是否系统内置字典：1=是，0=否，默认 0。</summary>
    public string? IsSys { get; set; } = "0";
    /// <summary>字典类型排序（数值越小越靠前）。</summary>
    public decimal? Sort { get; set; }
}
