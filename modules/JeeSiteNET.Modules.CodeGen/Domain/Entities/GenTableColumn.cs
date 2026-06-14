    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.CodeGen.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.CodeGen.Domain.Entities
namespace JeeSiteNET.Modules.CodeGen.Domain.Entities;

// 定义class GenTableColumn
// 定义类：GenTableColumn
public class GenTableColumn : BaseEntity
{
    // 属性 ColumnId
    // 属性：ColumnId
    public string ColumnId { get; set; } = string.Empty;
    // 属性 TableName
    // 属性：TableName
    public string TableName { get; set; } = string.Empty;
    // 属性 ColumnName
    // 属性：ColumnName
    public string ColumnName { get; set; } = string.Empty;
    // 属性：ColumnSort
    public int? ColumnSort { get; set; }
    // 属性：ColumnComment
    public string? ColumnComment { get; set; }
    // 属性：ColumnType
    public string? ColumnType { get; set; }
    // 属性：NetType
    public string? NetType { get; set; }
    // 属性：PropertyName
    public string? PropertyName { get; set; }
    // 属性：MaxLength
    public int? MaxLength { get; set; }
    // 属性：IsPk
    public string? IsPk { get; set; } = "0";
    // 属性：IsNullable
    public string? IsNullable { get; set; } = "1";
    // 属性：IsInsert
    public string? IsInsert { get; set; } = "1";
    // 属性：IsEdit
    public string? IsEdit { get; set; } = "1";
    // 属性：IsList
    public string? IsList { get; set; } = "1";
    // 属性：IsQuery
    public string? IsQuery { get; set; } = "0";
    // 属性：QueryType
    public string? QueryType { get; set; } = "EQ";
    // 属性：HtmlType
    public string? HtmlType { get; set; } = "input";
    // 属性：DictType
    public string? DictType { get; set; }
    // 属性：Status
    public string? Status { get; set; } = "0";
}
