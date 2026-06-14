using System.Globalization;

namespace JeeSiteNET.Core.Utils.ExcelFieldTypes;

/// <summary>
/// 高精度小数（decimal）字段类型。
/// Format 为标准 .NET 数字格式字符串（如 "0.00"、"#,##0.00"）。
/// 导出：decimal → 格式化字符串；导入：字符串 → decimal（去除货币符号）。
/// </summary>
public class DecimalFieldType : IExcelFieldType
{
    /// <summary>目标属性类型：decimal。</summary>
    public Type ValueType => typeof(decimal);

    /// <summary>数字格式字符串，默认 "0.00"（保留 2 位小数）。</summary>
    public string Format { get; init; } = "0.00";

    /// <summary>
    /// 导出：按 Format 格式化 decimal 值。
    /// </summary>
    /// <param name="value">原始数值（decimal、double、int 等均可）。</param>
    /// <returns>格式化字符串；无法解析返回原始 ToString。</returns>
    public string? ValueToCell(object? value)
    {
        if (value == null) return null;
        if (decimal.TryParse(value.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out var d))
            return d.ToString(Format, CultureInfo.InvariantCulture);
        return value.ToString();
    }

    /// <summary>
    /// 导入：将文本解析为 decimal。
    /// </summary>
    /// <param name="cellText">单元格文本。</param>
    /// <returns>decimal；无法解析返回 null。</returns>
    public object? CellToValue(string cellText)
    {
        if (string.IsNullOrWhiteSpace(cellText)) return null;
        if (decimal.TryParse(cellText, NumberStyles.Number, CultureInfo.InvariantCulture, out var d))
            return d;
        return null;
    }
}
