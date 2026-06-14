namespace JeeSiteNET.Core;

/// <summary>
/// 扩展字段实体接口：为业务实体预留 8 个字符串、4 个整型、4 个小数、4 个日期、1 个 JSON 扩展字段，
/// 便于在不修改数据库结构的前提下存储自定义属性
/// </summary>
public interface IExtendEntity
{
    /// <summary>
    /// 字符串扩展字段 1
    /// </summary>
    string? ExtendS1 { get; set; }

    /// <summary>
    /// 字符串扩展字段 2
    /// </summary>
    string? ExtendS2 { get; set; }

    /// <summary>
    /// 字符串扩展字段 3
    /// </summary>
    string? ExtendS3 { get; set; }

    /// <summary>
    /// 字符串扩展字段 4
    /// </summary>
    string? ExtendS4 { get; set; }

    /// <summary>
    /// 字符串扩展字段 5
    /// </summary>
    string? ExtendS5 { get; set; }

    /// <summary>
    /// 字符串扩展字段 6
    /// </summary>
    string? ExtendS6 { get; set; }

    /// <summary>
    /// 字符串扩展字段 7
    /// </summary>
    string? ExtendS7 { get; set; }

    /// <summary>
    /// 字符串扩展字段 8
    /// </summary>
    string? ExtendS8 { get; set; }

    /// <summary>
    /// 整型扩展字段 1
    /// </summary>
    int? ExtendI1 { get; set; }

    /// <summary>
    /// 整型扩展字段 2
    /// </summary>
    int? ExtendI2 { get; set; }

    /// <summary>
    /// 整型扩展字段 3
    /// </summary>
    int? ExtendI3 { get; set; }

    /// <summary>
    /// 整型扩展字段 4
    /// </summary>
    int? ExtendI4 { get; set; }

    /// <summary>
    /// 小数扩展字段 1
    /// </summary>
    decimal? ExtendF1 { get; set; }

    /// <summary>
    /// 小数扩展字段 2
    /// </summary>
    decimal? ExtendF2 { get; set; }

    /// <summary>
    /// 小数扩展字段 3
    /// </summary>
    decimal? ExtendF3 { get; set; }

    /// <summary>
    /// 小数扩展字段 4
    /// </summary>
    decimal? ExtendF4 { get; set; }

    /// <summary>
    /// 日期扩展字段 1
    /// </summary>
    DateTime? ExtendD1 { get; set; }

    /// <summary>
    /// 日期扩展字段 2
    /// </summary>
    DateTime? ExtendD2 { get; set; }

    /// <summary>
    /// 日期扩展字段 3
    /// </summary>
    DateTime? ExtendD3 { get; set; }

    /// <summary>
    /// 日期扩展字段 4
    /// </summary>
    DateTime? ExtendD4 { get; set; }

    /// <summary>
    /// JSON 格式扩展字段：用于存储结构化的自定义对象
    /// </summary>
    string? ExtendJson { get; set; }
}
