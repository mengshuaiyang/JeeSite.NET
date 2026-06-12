using System.Globalization;

namespace JeeSiteNET.Core.Utils.ExcelFieldTypes;

/// <summary>
/// 日期时间字段类型。
/// Format: "yyyy-MM-dd HH:mm:ss" / "yyyy-MM-dd" / ...
/// </summary>
public class DateTimeFieldType : IExcelFieldType
{
    public Type ValueType => typeof(DateTime);
    public string Format { get; init; } = "yyyy-MM-dd HH:mm:ss";

    public string? ValueToCell(object? value)
    {
        if (value == null) return null;
        if (value is DateTime dt) return dt.ToString(Format, CultureInfo.InvariantCulture);
        if (DateTime.TryParse(value.ToString(), out var parsed))
            return parsed.ToString(Format, CultureInfo.InvariantCulture);
        return value.ToString();
    }

    public object? CellToValue(string cellText)
    {
        if (string.IsNullOrWhiteSpace(cellText)) return null;
        if (DateTime.TryParseExact(cellText, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            return dt;
        if (DateTime.TryParse(cellText, out var fallback))
            return fallback;
        return null;
    }
}
