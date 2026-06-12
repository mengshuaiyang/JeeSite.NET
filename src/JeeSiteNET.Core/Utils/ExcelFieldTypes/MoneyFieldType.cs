using System.Globalization;

namespace JeeSiteNET.Core.Utils.ExcelFieldTypes;

/// <summary>
/// 金额字段类型。
/// 以 2 位小数 + 千分位格式呈现；导入时自动去除货币符号与千分位分隔符。
/// </summary>
public class MoneyFieldType : IExcelFieldType
{
    public Type ValueType => typeof(decimal);
    public string CurrencySymbol { get; init; } = "¥";
    public int Decimals { get; init; } = 2;
    public bool ShowCurrency { get; init; } = false;

    public string? ValueToCell(object? value)
    {
        if (value == null) return null;
        if (!decimal.TryParse(value.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out var d))
            return value.ToString();

        var format = "#,##0." + new string('0', Decimals);
        var str = d.ToString(format, CultureInfo.InvariantCulture);
        return ShowCurrency ? CurrencySymbol + str : str;
    }

    public object? CellToValue(string cellText)
    {
        if (string.IsNullOrWhiteSpace(cellText)) return null;
        var cleaned = new string(cellText
            .Where(c => char.IsDigit(c) || c == '.' || c == '-' || c == ',')
            .ToArray());
        if (decimal.TryParse(cleaned, NumberStyles.Number, CultureInfo.InvariantCulture, out var d))
            return d;
        return null;
    }
}
