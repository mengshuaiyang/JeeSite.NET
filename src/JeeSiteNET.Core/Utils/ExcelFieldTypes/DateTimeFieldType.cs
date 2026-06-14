using System.Globalization;

namespace JeeSiteNET.Core.Utils.ExcelFieldTypes;

/// <summary>
/// 日期时间字段类型。默认格式 "yyyy-MM-dd HH:mm:ss"，可通过 Format 属性覆盖。
/// 导出：DateTime → 格式化字符串；
/// 导入：字符串 → DateTime（先按 Format 精确解析，失败后回落到通用 DateTime.TryParse）。
/// </summary>
public class DateTimeFieldType : IExcelFieldType
{
    /// <summary>目标属性类型：DateTime。</summary>
    public Type ValueType => typeof(DateTime);

    /// <summary>.NET 标准格式字符串（默认 "yyyy-MM-dd HH:mm:ss"）。</summary>
    public string Format { get; init; } = "yyyy-MM-dd HH:mm:ss";

    /// <summary>
    /// 导出：将对象值按 Format 格式化为日期时间字符串。
    /// </summary>
    /// <param name="value">原始值（DateTime 或可解析为 DateTime 的字符串）。</param>
    /// <returns>格式化字符串；值为 null 返回 null。</returns>
    public string? ValueToCell(object? value)
    {
        if (value == null) return null;
        // 已是 DateTime → 直接格式化
        if (value is DateTime dt) return dt.ToString(Format, CultureInfo.InvariantCulture);
        // 字符串形式 → 先解析再格式化
        if (DateTime.TryParse(value.ToString(), out var parsed))
            return parsed.ToString(Format, CultureInfo.InvariantCulture);
        // 无法解析 → 保留原始字符串让上层展示
        return value.ToString();
    }

    /// <summary>
    /// 导入：将单元格文本按 Format 精确解析为 DateTime，失败后回落到通用解析。
    /// </summary>
    /// <param name="cellText">单元格文本。</param>
    /// <returns>解析后的 DateTime；无法解析返回 null。</returns>
    public object? CellToValue(string cellText)
    {
        if (string.IsNullOrWhiteSpace(cellText)) return null;
        // 先按精确格式解析
        if (DateTime.TryParseExact(cellText, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            return dt;
        // 回落通用解析（兼容 Excel 默认日期/时间格式）
        if (DateTime.TryParse(cellText, out var fallback))
            return fallback;
        return null;
    }
}
