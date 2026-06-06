using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.CodeGen.Domain.Entities;

public class GenTable : BaseEntity
{
    public string TableName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string ModuleCode { get; set; } = string.Empty;
    public string? FunctionName { get; set; }
    public string? FunctionAuthor { get; set; }
    public string? TableComment { get; set; }
    public string? ParentTableName { get; set; }
    public string? ParentFieldName { get; set; }
    public string? TplCategory { get; set; } = "crud";
    public string? PackageName { get; set; }
    public string? BusinessName { get; set; }
    public string? TreeCode { get; set; }
    public string? TreeParentCode { get; set; }
    public string? TreeName { get; set; }
    public string? Options { get; set; }
    public string? Status { get; set; } = "0";
    public List<GenTableColumn> Columns { get; set; } = [];
}
