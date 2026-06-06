using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.CodeGen.Domain.Entities;

public class GenTableColumn : BaseEntity
{
    public string ColumnId { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public string ColumnName { get; set; } = string.Empty;
    public int? ColumnSort { get; set; }
    public string? ColumnComment { get; set; }
    public string? ColumnType { get; set; }
    public string? NetType { get; set; }
    public string? PropertyName { get; set; }
    public int? MaxLength { get; set; }
    public string? IsPk { get; set; } = "0";
    public string? IsNullable { get; set; } = "1";
    public string? IsInsert { get; set; } = "1";
    public string? IsEdit { get; set; } = "1";
    public string? IsList { get; set; } = "1";
    public string? IsQuery { get; set; } = "0";
    public string? QueryType { get; set; } = "EQ";
    public string? HtmlType { get; set; } = "input";
    public string? DictType { get; set; }
    public string? Status { get; set; } = "0";
}
