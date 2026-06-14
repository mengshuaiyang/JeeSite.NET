using System.Globalization;

namespace JeeSiteNET.Core.Utils.ExcelFieldTypes;

/// <summary>
/// 金额字段类型。以 N 位小数 + 千分位格式呈现；导入时自动去除货币符号与千分位分隔符。
/// 自定义参数：CurrencySymbol（默认 "¥"）、Decimals（默认 2）、ShowCurrency（默认 false，是否在导出时附加货币符号）。
/// </summary>
public class MoneyFieldType : IExcelFieldType
{
    /// <summary>目标属性类型：decimal。</summary>
    public Type ValueType => typeof(decimal);

    /// <summary>货币符号（默认 "¥"），仅在 ShowCurrency=true 时输出。</summary>
    public string CurrencySymbol { get; init; } = "¥";

    /// <summary>小数位数（默认 2），决定 #,##0.00... 的格式。</summary>
    public int Decimals { get; init; } = 2;

    /// <summary>是否在导出时附加货币符号（默认 false）。</summary>
    public bool ShowCurrency { get; init; } = false;

    /// <summary>
    /// 导出：decimal → 千分位格式化字符串（可选货币前缀）。
    /// </summary>
    /// <param name="value">金额数值。</param>
    /// <returns>格式化字符串；无法解析返回原始 ToString。</returns>
    public string? ValueToCell(object? value)
    {
        if (value == null) return null;
        if (!decimal.TryParse(value.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out var d))
            return value.ToString();

        var format = "#,##0." + new string('0', Decimals);
        var str = d.ToString(format, CultureInfo.InvariantCulture);
        return ShowCurrency ? CurrencySymbol + str : str;
    }

    /// <summary>
    /// 导入：去除货币符号、千分位分隔符等非数字字符，解析为 decimal。
    /// </summary>
    /// <param name="cellText">单元格文本（可能含 ¥ $ , 等）。</param>
    /// <returns>decimal；无法解析返回 null。</returns>
    public object? CellToValue(string cellText)
    {
        if (string.IsNullOrWhiteSpace(cellText)) return null;
        // 仅保留数字、小数点、负号、千分位逗号
        var cleaned = new string(cellText
            .Where(c => char.IsDigit(c) || c == '.' || c == '-' || c == ',')
            .ToArray());
        if (decimal.TryParse(cleaned, NumberStyles.Number, CultureInfo.InvariantCulture, out var d))
            return d;
        return null;
    }
}
