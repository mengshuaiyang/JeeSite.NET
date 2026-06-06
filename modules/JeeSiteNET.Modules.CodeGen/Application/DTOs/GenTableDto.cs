using JeeSiteNET.Modules.CodeGen.Domain.Entities;

namespace JeeSiteNET.Modules.CodeGen.Application.DTOs;

public class GenTableDto
{
    public string TableName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string ModuleCode { get; set; } = string.Empty;
    public string? FunctionName { get; set; }
    public string? FunctionAuthor { get; set; }
    public string? TableComment { get; set; }
    public string? TplCategory { get; set; }
    public string? BusinessName { get; set; }
    public string? Status { get; set; }
    public List<GenTableColumnDto> Columns { get; set; } = [];

    public static GenTableDto FromEntity(GenTable e) => new()
    {
        TableName = e.TableName, ClassName = e.ClassName, ModuleCode = e.ModuleCode,
        FunctionName = e.FunctionName, FunctionAuthor = e.FunctionAuthor,
        TableComment = e.TableComment, TplCategory = e.TplCategory,
        BusinessName = e.BusinessName, Status = e.Status,
        Columns = e.Columns?.Select(GenTableColumnDto.FromEntity).ToList() ?? []
    };
}

public class GenTableSaveDto
{
    public string TableName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string ModuleCode { get; set; } = string.Empty;
    public string? FunctionName { get; set; }
    public string? FunctionAuthor { get; set; }
    public string? TableComment { get; set; }
    public string? TplCategory { get; set; } = "crud";
    public string? BusinessName { get; set; }
    public string? Status { get; set; } = "0";
    public List<GenTableColumnSaveDto> Columns { get; set; } = [];
}

public class DbTableInfo
{
    public string TableName { get; set; } = string.Empty;
    public string? TableComment { get; set; }
    public string? CreateTime { get; set; }
}

public class ColumnInfo
{
    public string ColumnName { get; set; } = string.Empty;
    public string? ColumnComment { get; set; }
    public string? ColumnType { get; set; }
    public string? NetType { get; set; }
    public string? IsNullable { get; set; }
    public int? MaxLength { get; set; }
    public string? IsPk { get; set; } = "0";
}
