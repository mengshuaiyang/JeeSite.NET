using System.Globalization;

namespace JeeSiteNET.Core.Utils.ExcelFieldTypes;

/// <summary>
/// 高精度小数（decimal）字段类型。
/// Format: "0.00" / "#,##0.00" / ... — 标准 .NET 数字格式字符串
/// </summary>
public class DecimalFieldType : IExcelFieldType
{
    public Type ValueType => typeof(decimal);
    public string Format { get; init; } = "0.00";

    public string? ValueToCell(object? value)
    {
        if (value == null) return null;
        if (decimal.TryParse(value.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out var d))
            return d.ToString(Format, CultureInfo.InvariantCulture);
        return value.ToString();
    }

    public object? CellToValue(string cellText)
    {
        if (string.IsNullOrWhiteSpace(cellText)) return null;
        if (decimal.TryParse(cellText, NumberStyles.Number, CultureInfo.InvariantCulture, out var d))
            return d;
        return null;
    }
}
