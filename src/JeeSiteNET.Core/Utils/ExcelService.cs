using System.Data;
using System.Reflection;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;

namespace JeeSiteNET.Core.Utils;

public class ExcelService
{
    public byte[] Export<T>(List<T> data, string sheetName = "Sheet1")
    {
        var props = GetExportableProps(typeof(T));
        var workbook = new XSSFWorkbook();
        var sheet = workbook.CreateSheet(sheetName);
        var headerStyle = CreateHeaderStyle(workbook);

        var headerRow = sheet.CreateRow(0);
        for (int i = 0; i < props.Count; i++)
        {
            headerRow.CreateCell(i).SetCellValue(props[i].GetTitle());
            headerRow.Cells[i].CellStyle = headerStyle;
            sheet.SetColumnWidth(i, (int)(props[i].GetColumnWidth() * 256));
        }

        for (int r = 0; r < data.Count; r++)
        {
            var row = sheet.CreateRow(r + 1);
            var item = data[r];
            if (item == null) continue;
            for (int c = 0; c < props.Count; c++)
            {
                var val = props[c].Getter(item);
                var cell = row.CreateCell(c);
                SetCellValue(cell, val, props[c]);
            }
        }

        using var ms = new MemoryStream();
        workbook.Write(ms);
        return ms.ToArray();
    }

    public byte[] ExportTemplate<T>(string sheetName = "Sheet1")
    {
        var props = GetExportableProps(typeof(T));
        var workbook = new XSSFWorkbook();
        var sheet = workbook.CreateSheet(sheetName);
        var headerStyle = CreateHeaderStyle(workbook);

        var headerRow = sheet.CreateRow(0);
        for (int i = 0; i < props.Count; i++)
        {
            headerRow.CreateCell(i).SetCellValue(props[i].GetTitle());
            headerRow.Cells[i].CellStyle = headerStyle;
            sheet.SetColumnWidth(i, (int)(props[i].GetColumnWidth() * 256));
        }

        using var ms = new MemoryStream();
        workbook.Write(ms);
        return ms.ToArray();
    }

    public List<T> Import<T>(byte[] bytes, string? sheetName = null) where T : new()
    {
        IWorkbook workbook = bytes[0] == 0x50
            ? new XSSFWorkbook(new MemoryStream(bytes))
            : new HSSFWorkbook(new MemoryStream(bytes));
        var sheet = sheetName != null ? workbook.GetSheet(sheetName) : workbook.GetSheetAt(0);
        if (sheet == null) return [];

        var props = GetExportableProps(typeof(T));
        var headerMap = new Dictionary<string, ExcelPropInfo>();
        var headerRow = sheet.GetRow(0);
        if (headerRow == null) return [];

        for (int c = 0; c < headerRow.LastCellNum; c++)
        {
            var title = headerRow.GetCell(c)?.StringCellValue?.Trim();
            if (!string.IsNullOrEmpty(title))
            {
                var prop = props.FirstOrDefault(p => p.GetTitle() == title);
                if (prop != null)
                    headerMap[title] = prop;
            }
        }

        var result = new List<T>();
        for (int r = 1; r <= sheet.LastRowNum; r++)
        {
            var row = sheet.GetRow(r);
            if (row == null) continue;
            var item = new T();
            bool hasValue = false;

            foreach (var kv in headerMap)
            {
                var cell = row.GetCell(Array.IndexOf(props.ToArray(), kv.Value));
                // Find column index
                int colIdx = -1;
                for (int c = 0; c < headerRow.LastCellNum; c++)
                {
                    if (headerRow.GetCell(c)?.StringCellValue?.Trim() == kv.Key)
                    {
                        colIdx = c;
                        break;
                    }
                }
                if (colIdx < 0) continue;

                cell = row.GetCell(colIdx);
                if (cell == null) continue;

                var val = GetCellValue(cell, kv.Value, kv.Value.PropType);
                if (val != null && (val.ToString() != "" || kv.Value.PropType != typeof(string)))
                {
                    kv.Value.Setter(item, val);
                    hasValue = true;
                }
            }

            if (hasValue)
                result.Add(item);
        }

        return result;
    }

    private static List<ExcelPropInfo> GetExportableProps(Type type)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => new ExcelPropInfo(p))
            .Where(p => p.IsExport)
            .OrderBy(p => p.Sort)
            .ToList();
    }

    private static ICellStyle CreateHeaderStyle(IWorkbook workbook)
    {
        var style = workbook.CreateCellStyle();
        var font = workbook.CreateFont();
        font.IsBold = true;
        style.SetFont(font);
        style.FillForegroundColor = IndexedColors.Grey25Percent.Index;
        style.FillPattern = FillPattern.SolidForeground;
        style.BorderBottom = BorderStyle.Thin;
        style.BorderTop = BorderStyle.Thin;
        style.BorderLeft = BorderStyle.Thin;
        style.BorderRight = BorderStyle.Thin;
        return style;
    }

    private static void SetCellValue(ICell cell, object? val, ExcelPropInfo prop)
    {
        var fieldType = prop.GetFieldTypeInstance();
        if (fieldType != null)
        {
            var iface = fieldType.GetType().GetInterface("IExcelFieldType");
            if (iface != null)
            {
                var valueToCell = iface.GetMethod("ValueToCell");
                var formatted = valueToCell?.Invoke(fieldType, new[] { val }) as string;
                if (formatted != null) { cell.SetCellValue(formatted); return; }
            }
        }

        if (val == null) { cell.SetCellValue(string.Empty); return; }

        if (prop.PropType == typeof(int) || prop.PropType == typeof(long) ||
            prop.PropType == typeof(short) || prop.PropType == typeof(byte))
            cell.SetCellValue(Convert.ToDouble(val));
        else if (prop.PropType == typeof(decimal) || prop.PropType == typeof(double) || prop.PropType == typeof(float))
            cell.SetCellValue(Convert.ToDouble(val));
        else if (prop.PropType == typeof(DateTime) || prop.PropType == typeof(DateTime?))
        {
            var dt = (DateTime)val;
            if (!string.IsNullOrEmpty(prop.DataFormat))
            {
                var style = cell.Sheet.Workbook.CreateCellStyle();
                style.DataFormat = cell.Sheet.Workbook.CreateDataFormat().GetFormat(prop.DataFormat);
                cell.CellStyle = style;
            }
            cell.SetCellValue(dt);
        }
        else if (prop.PropType == typeof(bool))
            cell.SetCellValue((bool)val);
        else
            cell.SetCellValue(val?.ToString() ?? "");
    }

    private static object? GetCellValue(ICell cell, ExcelPropInfo prop, Type targetType)
    {
        if (cell == null) return null;

        var fieldType = prop.GetFieldTypeInstance();
        if (fieldType != null)
        {
            var iface = fieldType.GetType().GetInterface("IExcelFieldType");
            if (iface != null)
            {
                var cellToValue = iface.GetMethod("CellToValue");
                var rawText = GetRawCellText(cell);
                var val = cellToValue?.Invoke(fieldType, new[] { rawText });
                if (val != null) return val;
            }
        }

        switch (cell.CellType)
        {
            case CellType.Numeric:
                if (targetType == typeof(int) || targetType == typeof(int?))
                    return (int)cell.NumericCellValue;
                if (targetType == typeof(long) || targetType == typeof(long?))
                    return (long)cell.NumericCellValue;
                if (targetType == typeof(decimal) || targetType == typeof(decimal?))
                    return (decimal)cell.NumericCellValue;
                if (targetType == typeof(double) || targetType == typeof(double?))
                    return cell.NumericCellValue;
                if (targetType == typeof(DateTime) || targetType == typeof(DateTime?))
                    return cell.DateCellValue;
                return cell.NumericCellValue;

            case CellType.String:
                var str = cell.StringCellValue;
                if (targetType == typeof(int) || targetType == typeof(int?))
                    return int.TryParse(str, out var iv) ? iv : null;
                if (targetType == typeof(long) || targetType == typeof(long?))
                    return long.TryParse(str, out var lv) ? lv : null;
                if (targetType == typeof(decimal) || targetType == typeof(decimal?))
                    return decimal.TryParse(str, out var dv) ? dv : null;
                if (targetType == typeof(DateTime) || targetType == typeof(DateTime?))
                    return DateTime.TryParse(str, out var dtv) ? dtv : null;
                if (targetType == typeof(bool) || targetType == typeof(bool?))
                    return str == "是" || str == "true" || str == "1";
                if (targetType.IsEnum)
                    return Enum.TryParse(targetType, str, true, out var ev) ? ev : null;
                return str;

            case CellType.Boolean:
                if (targetType == typeof(bool) || targetType == typeof(bool?))
                    return cell.BooleanCellValue;
                return cell.BooleanCellValue ? "是" : "否";

            case CellType.Formula:
                try
                {
                    if (targetType == typeof(DateTime) || targetType == typeof(DateTime?))
                        return cell.DateCellValue;
                    return cell.StringCellValue;
                }
                catch { return cell.NumericCellValue; }
        }
        return null;
    }

    private static string GetRawCellText(ICell cell)
    {
        try
        {
            return cell.CellType switch
            {
                CellType.Numeric => cell.NumericCellValue.ToString(),
                CellType.String => cell.StringCellValue,
                CellType.Boolean => cell.BooleanCellValue.ToString(),
                CellType.Formula => cell.StringCellValue ?? cell.NumericCellValue.ToString(),
                _ => ""
            };
        }
        catch { return ""; }
    }

    private class ExcelPropInfo
    {
        private readonly PropertyInfo _prop;
        private readonly ExcelFieldAttribute? _attr;
        private Type? FieldType { get; }
        private object? _fieldTypeInstance;

        public int Sort => _attr?.Sort ?? 999;
        public bool IsExport => _attr?.IsExport ?? true;
        public bool IsImport => _attr?.IsImport ?? true;
        public Type PropType => _prop.PropertyType;
        public string? DataFormat => _attr?.DataFormat;

        public ExcelPropInfo(PropertyInfo prop)
        {
            _prop = prop;
            _attr = prop.GetCustomAttribute<ExcelFieldAttribute>();
            FieldType = _attr?.FieldType;
        }

        public object? GetFieldTypeInstance()
        {
            if (FieldType == null) return null;
            if (_fieldTypeInstance != null) return _fieldTypeInstance;
            try
            {
                _fieldTypeInstance = Activator.CreateInstance(FieldType);
            }
            catch
            {
                _fieldTypeInstance = null;
            }
            return _fieldTypeInstance;
        }

        public string GetTitle() => _attr?.Title ?? _prop.Name;
        public double GetColumnWidth() => _attr?.ColumnWidth ?? 20;
        public object? Getter(object obj) => _prop.GetValue(obj)!;
        public void Setter(object obj, object? val) => _prop.SetValue(obj, val);
    }
}
