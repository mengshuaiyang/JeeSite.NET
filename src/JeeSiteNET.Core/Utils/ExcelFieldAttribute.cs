namespace JeeSiteNET.Core.Utils;

/// <summary>
/// Excel 列元数据注解。用于控制 ExcelService 导入/导出时的列标题、顺序、格式与宽度。
/// 可搭配 FieldType 指定自定义字段类型（如 MoneyFieldType、AreaFieldType 等）。
///
/// 示例：
/// <code>
/// [ExcelField("工资", FieldType = typeof(MoneyFieldType))]
/// public decimal Salary { get; set; }
/// </code>
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ExcelFieldAttribute : Attribute
{
    /// <summary>
    /// 列标题（显示在 Excel 表头）。
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// 列排序序号（越小越靠前），默认值 999。
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 数据格式字符串（如 "yyyy-MM-dd"、"0.00"）。null 表示使用默认格式。
    /// </summary>
    public string? DataFormat { get; set; }

    /// <summary>
    /// 是否参与导出，默认值 true。
    /// </summary>
    public bool IsExport { get; set; } = true;

    /// <summary>
    /// 是否参与导入，默认值 true。
    /// </summary>
    public bool IsImport { get; set; } = true;

    /// <summary>
    /// 列宽（字符数），默认值 20。
    /// </summary>
    public double ColumnWidth { get; set; } = 20;

    /// <summary>
    /// 自定义字段类型（需实现 IExcelFieldType 接口）。指定后，该字段的导出/导入会委派给类型处理。
    /// </summary>
    public Type? FieldType { get; set; }

    /// <summary>
    /// 初始化一个 Excel 列元数据注解。
    /// </summary>
    /// <param name="title">列标题</param>
    public ExcelFieldAttribute(string title) => Title = title;
}
