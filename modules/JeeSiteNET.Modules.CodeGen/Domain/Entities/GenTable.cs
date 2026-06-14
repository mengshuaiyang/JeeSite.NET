    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.CodeGen.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.CodeGen.Domain.Entities
namespace JeeSiteNET.Modules.CodeGen.Domain.Entities;

// 定义class GenTable
// 定义类：GenTable
public class GenTable : BaseEntity
{
    // 属性 TableName
    // 属性：TableName
    public string TableName { get; set; } = string.Empty;
    // 属性 ClassName
    // 属性：ClassName
    public string ClassName { get; set; } = string.Empty;
    // 属性 ModuleCode
    // 属性：ModuleCode
    public string ModuleCode { get; set; } = string.Empty;
    // 属性：FunctionName
    public string? FunctionName { get; set; }
    // 属性：FunctionAuthor
    public string? FunctionAuthor { get; set; }
    // 属性：TableComment
    public string? TableComment { get; set; }
    // 属性：ParentTableName
    public string? ParentTableName { get; set; }
    // 属性：ParentFieldName
    public string? ParentFieldName { get; set; }
    // 属性：TplCategory
    public string? TplCategory { get; set; } = "crud";
    // 属性：PackageName
    public string? PackageName { get; set; }
    // 属性：BusinessName
    public string? BusinessName { get; set; }
    // 属性：TreeCode
    public string? TreeCode { get; set; }
    // 属性：TreeParentCode
    public string? TreeParentCode { get; set; }
    // 属性：TreeName
    public string? TreeName { get; set; }
    // 属性：Options
    public string? Options { get; set; }
    // 属性：Status
    public string? Status { get; set; } = "0";
    // 属性 Columns
    // 属性：Columns
    public List<GenTableColumn> Columns { get; set; } = [];
}
