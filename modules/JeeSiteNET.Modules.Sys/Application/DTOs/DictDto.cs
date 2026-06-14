namespace JeeSiteNET.Modules.Sys.Application.DTOs;

/// <summary>
/// 字典类型 DTO。
/// </summary>
public class DictTypeDto
{
    /// <summary>
    /// 字典类型编码（如 sys_user_status）。
    /// </summary>
    public string DictTypeCode { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型名称。
    /// </summary>
    public string DictName { get; set; } = string.Empty;

    /// <summary>
    /// 是否系统内置（1 是 / 0 否）。
    /// </summary>
    public string? IsSys { get; set; }

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
/// 字典类型保存请求 DTO。
/// </summary>
public class DictTypeSaveDto
{
    /// <summary>
    /// 字典类型编码；空表示新建。
    /// </summary>
    public string? DictTypeCode { get; set; }

    /// <summary>
    /// 字典类型名称。
    /// </summary>
    public string DictName { get; set; } = string.Empty;

    /// <summary>
    /// 是否系统内置。
    /// </summary>
    public string? IsSys { get; set; }

    /// <summary>
    /// 排序值。
    /// </summary>
    public decimal? Sort { get; set; }
}

/// <summary>
/// 字典条目/值 DTO。
/// </summary>
public class DictDataDto
{
    /// <summary>
    /// 字典条目编码（主键）。
    /// </summary>
    public string DictCode { get; set; } = string.Empty;

    /// <summary>
    /// 所属字典类型。
    /// </summary>
    public string DictType { get; set; } = string.Empty;

    /// <summary>
    /// 显示文本。
    /// </summary>
    public string DictLabel { get; set; } = string.Empty;

    /// <summary>
    /// 实际值。
    /// </summary>
    public string DictValue { get; set; } = string.Empty;

    /// <summary>
    /// 同级排序。
    /// </summary>
    public decimal? Sort { get; set; }

    /// <summary>
    /// 父级字典条目编码（用于树形字典）。
    /// </summary>
    public string? ParentCode { get; set; }

    /// <summary>
    /// 是否叶子节点（1 是 / 0 否）。
    /// </summary>
    public string? TreeLeaf { get; set; }

    /// <summary>
    /// 状态（0 正常 / 1 禁用）。
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// 子条目集合（树形字典用）。
    /// </summary>
    public List<DictDataDto> Children { get; set; } = new();
}

/// <summary>
/// 字典条目保存请求 DTO。
/// </summary>
public class DictDataSaveDto
{
    /// <summary>
    /// 字典条目编码；空表示新建。
    /// </summary>
    public string? DictCode { get; set; }

    /// <summary>
    /// 所属字典类型。
    /// </summary>
    public string DictType { get; set; } = string.Empty;

    /// <summary>
    /// 显示文本。
    /// </summary>
    public string DictLabel { get; set; } = string.Empty;

    /// <summary>
    /// 实际值。
    /// </summary>
    public string DictValue { get; set; } = string.Empty;

    /// <summary>
    /// 父级字典条目编码。
    /// </summary>
    public string? ParentCode { get; set; }

    /// <summary>
    /// 排序值。
    /// </summary>
    public decimal? Sort { get; set; }
}
