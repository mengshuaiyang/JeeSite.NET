namespace JeeSiteNET.Core.Utils;

[AttributeUsage(AttributeTargets.Property)]
public class ExcelFieldAttribute : Attribute
{
    public string Title { get; }
    public int Sort { get; set; }
    public string? DataFormat { get; set; }
    public bool IsExport { get; set; } = true;
    public bool IsImport { get; set; } = true;
    public double ColumnWidth { get; set; } = 20;

    public ExcelFieldAttribute(string title) => Title = title;
}
