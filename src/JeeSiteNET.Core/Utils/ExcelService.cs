using System.Reflection;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// Excel 导入/导出服务。基于实体类上的 <see cref="ExcelFieldAttribute"/> 注解
/// 决定列元数据（列名、顺序、列宽、格式化字符串等）。导出使用 XSSF（.xlsx），
/// 导入兼容 XSSF 与 HSSF（.xls）。
/// </summary>
public class ExcelService
{
    /// <summary>
    /// 将实体列表导出为 Excel 文件字节（.xlsx 格式）。
    /// 列标题取自 <see cref="ExcelFieldAttribute.Title"/>（缺省使用属性名）。
    /// </summary>
    /// <typeparam name="T">实体类型。</typeparam>
    /// <param name="data">要导出的数据列表。</param>
    /// <param name="sheetName">工作表名称，默认 "Sheet1"。</param>
    /// <returns>Excel 文件的字节数组（可直接写入 Response 或文件）。</returns>
    public byte[] Export<T>(List<T> data, string sheetName = "Sheet1")
    {
        var props = GetExportableProps(typeof(T));
        var workbook = new XSSFWorkbook();
        var sheet = workbook.CreateSheet(sheetName);
        var headerStyle = CreateHeaderStyle(workbook);

        // 第 0 行：表头
        var headerRow = sheet.CreateRow(0);
        for (int i = 0; i < props.Count; i++)
        {
            headerRow.CreateCell(i).SetCellValue(props[i].GetTitle());
            headerRow.Cells[i].CellStyle = headerStyle;
            sheet.SetColumnWidth(i, (int)(props[i].GetColumnWidth() * 256));
        }

        // 第 1 行起：数据行
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

    /// <summary>
    /// 生成仅包含表头的 Excel 模板文件（用于用户下载填写）。
    /// </summary>
    /// <typeparam name="T">实体类型。</typeparam>
    /// <param name="sheetName">工作表名称，默认 "Sheet1"。</param>
    /// <returns>Excel 文件的字节数组。</returns>
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

    /// <summary>
    /// 从 Excel 文件字节中读取实体列表。首行作为表头，
    /// 根据 <see cref="ExcelFieldAttribute.Title"/> 与表头文本匹配，
    /// 决定每一列映射到哪个属性。
    /// </summary>
    /// <typeparam name="T">实体类型，必须具有无参构造函数。</typeparam>
    /// <param name="bytes">Excel 文件字节（.xlsx 或 .xls）。</param>
    /// <param name="sheetName">指定工作表名；null 表示第一张工作表。</param>
    /// <returns>实体列表。空表或解析无结果时返回空列表（非 null）。</returns>
    public List<T> Import<T>(byte[] bytes, string? sheetName = null) where T : new()
    {
        // 根据文件首字节判断文件格式（0x50 = 'P' 是 ZIP，即 .xlsx；否则当作 .xls 处理）
        IWorkbook workbook = bytes.Length > 0 && bytes[0] == 0x50
            ? new XSSFWorkbook(new MemoryStream(bytes))
            : new HSSFWorkbook(new MemoryStream(bytes));

        var sheet = sheetName != null ? workbook.GetSheet(sheetName) : workbook.GetSheetAt(0);
        if (sheet == null) return [];

        var props = GetExportableProps(typeof(T));
        var headerMap = new Dictionary<string, ExcelPropInfo>();
        var headerRow = sheet.GetRow(0);
        if (headerRow == null) return [];

        // 建立「表头文本 → 属性元信息」映射
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
                // 以表头文本再次在表头行寻找对应列索引（不依赖顺序，更稳健）
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

                var cell = row.GetCell(colIdx);
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

    /// <summary>
    /// 从实体类型中提取带 <see cref="ExcelFieldAttribute"/> 注解的属性，
    /// 按 Sort 值排序。不含注解的属性默认不参与导入/导出。
    /// </summary>
    private static List<ExcelPropInfo> GetExportableProps(Type type)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => new ExcelPropInfo(p))
            .Where(p => p.IsExport)
            .OrderBy(p => p.Sort)
            .ToList();
    }

    /// <summary>
    /// 创建表头单元格样式（粗体 + 灰底 + 全边框）。
    /// </summary>
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

    /// <summary>
    /// 根据属性类型将对象值写入单元格。
    /// - 数值类型（int/long/decimal/double/float/short/byte）→ SetCellValue(double)
    /// - DateTime → 按 DataFormat 设置格式化并写入日期
    /// - bool → SetCellValue(bool)
    /// - string / 其它 → ToString()
    /// 若属性指定了 <see cref="ExcelFieldAttribute.FieldType"/>，则优先委派给
    /// 自定义字段类型（如 Money、Area 等）进行格式化输出。
    /// </summary>
    private static void SetCellValue(ICell cell, object? val, ExcelPropInfo prop)
    {
        var fieldType = prop.GetFieldTypeInstance();
        if (fieldType != null)
        {
            // 自定义字段类型：通过反射调用 IExcelFieldType.ValueToCell
            var iface = fieldType.GetType().GetInterface("IExcelFieldType");
            if (iface != null)
            {
                var valueToCell = iface.GetMethod("ValueToCell");
                var formatted = valueToCell?.Invoke(fieldType, new[] { val }) as string;
                if (formatted != null) { cell.SetCellValue(formatted); return; }
            }
        }

        if (val == null) { cell.SetCellValue(string.Empty); return; }

        // 数值类型统一转换为 double 以便 Excel 正确识别为数值
        if (prop.PropType == typeof(int) || prop.PropType == typeof(long) ||
            prop.PropType == typeof(short) || prop.PropType == typeof(byte))
            cell.SetCellValue(Convert.ToDouble(val));
        else if (prop.PropType == typeof(decimal) || prop.PropType == typeof(double) || prop.PropType == typeof(float))
            cell.SetCellValue(Convert.ToDouble(val));
        else if (prop.PropType == typeof(DateTime) || prop.PropType == typeof(DateTime?))
        {
            // 对日期类型，如在注解中指定 DataFormat，则应用格式
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

    /// <summary>
    /// 根据目标属性类型从单元格中读取值，并做必要的类型转换。
    /// 对于 Excel 文本/数字单元格，将按目标类型解析为 int/long/decimal/double/DateTime/Enum。
    /// </summary>
    private static object? GetCellValue(ICell cell, ExcelPropInfo prop, Type targetType)
    {
        if (cell == null) return null;

        // 自定义字段类型：通过反射调用 IExcelFieldType.CellToValue
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
                // 字符串 → 目标类型的常见转换
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

    /// <summary>
    /// 获取单元格的原始文本表达（用于自定义字段类型解析）。
    /// </summary>
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

    /// <summary>
    /// 单元格属性元数据：持有 PropertyInfo 与其 ExcelFieldAttribute，
    /// 提供方便的 Getter / Setter 与默认值推算。
    /// </summary>
    private class ExcelPropInfo
    {
        private readonly PropertyInfo _prop;
        private readonly ExcelFieldAttribute? _attr;
        private Type? FieldType { get; }
        private object? _fieldTypeInstance;

        /// <summary>
        /// 列排序，默认 999（即放到末尾）。
        /// </summary>
        public int Sort => _attr?.Sort ?? 999;

        /// <summary>
        /// 是否参与导出，默认 true。
        /// </summary>
        public bool IsExport => _attr?.IsExport ?? true;

        /// <summary>
        /// 是否参与导入，默认 true。
        /// </summary>
        public bool IsImport => _attr?.IsImport ?? true;

        /// <summary>
        /// 属性的原生类型。
        /// </summary>
        public Type PropType => _prop.PropertyType;

        /// <summary>
        /// DataFormat 格式化字符串（如 "yyyy-MM-dd"）。
        /// </summary>
        public string? DataFormat => _attr?.DataFormat;

        public ExcelPropInfo(PropertyInfo prop)
        {
            _prop = prop;
            _attr = prop.GetCustomAttribute<ExcelFieldAttribute>();
            FieldType = _attr?.FieldType;
        }

        /// <summary>
        /// 获取自定义字段类型的单例实例（懒加载，异常时降级为 null）。
        /// </summary>
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

        /// <summary>
        /// 获取列标题：优先用注解 Title，否则用属性名。
        /// </summary>
        public string GetTitle() => _attr?.Title ?? _prop.Name;

        /// <summary>
        /// 获取列宽（字符数 × 256 内部转换由外部完成）。
        /// </summary>
        public double GetColumnWidth() => _attr?.ColumnWidth ?? 20;

        /// <summary>
        /// 通过反射读取属性值。
        /// </summary>
        public object? Getter(object obj) => _prop.GetValue(obj)!;

        /// <summary>
        /// 通过反射设置属性值。
        /// </summary>
        public void Setter(object obj, object? val) => _prop.SetValue(obj, val);
    }
}
