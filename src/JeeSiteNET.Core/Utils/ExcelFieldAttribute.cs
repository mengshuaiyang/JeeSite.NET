namespace JeeSiteNET.Core.Utils;

/// <summary>
/// Excel 列元数据注解。可搭配 FieldType 指定自定义字段类型。
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
    public string Title { get; }
    public int Sort { get; set; }
    public string? DataFormat { get; set; }
    public bool IsExport { get; set; } = true;
    public bool IsImport { get; set; } = true;
    public double ColumnWidth { get; set; } = 20;

    /// <summary>
    /// 自定义字段类型（需实现 IExcelFieldType 接口）。
    /// 指定后，该字段的导出/导入会委派给类型处理。
    /// </summary>
    public Type? FieldType { get; set; }

    public ExcelFieldAttribute(string title) => Title = title;
}
