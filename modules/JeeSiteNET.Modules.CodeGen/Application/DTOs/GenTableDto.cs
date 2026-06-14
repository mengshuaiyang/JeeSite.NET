    // 引入 JeeSiteNET.Modules.CodeGen.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.CodeGen.Domain.Entities
using JeeSiteNET.Modules.CodeGen.Domain.Entities;

// 定义 JeeSiteNET.Modules.CodeGen.Application.DTOs 命名空间
// 定义命名空间：JeeSiteNET.Modules.CodeGen.Application.DTOs
namespace JeeSiteNET.Modules.CodeGen.Application.DTOs;

// 定义class GenTableDto
// 定义类：GenTableDto
public class GenTableDto
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
    // 属性：TplCategory
    public string? TplCategory { get; set; }
    // 属性：BusinessName
    public string? BusinessName { get; set; }
    // 属性：Status
    public string? Status { get; set; }
    // 属性 Columns
    // 属性：Columns
    public List<GenTableColumnDto> Columns { get; set; } = [];

    // 方法 FromEntity
    // 方法：FromEntity
    public static GenTableDto FromEntity(GenTable e) => new()
    {
        TableName = e.TableName, ClassName = e.ClassName, ModuleCode = e.ModuleCode,
        FunctionName = e.FunctionName, FunctionAuthor = e.FunctionAuthor,
        TableComment = e.TableComment, TplCategory = e.TplCategory,
        BusinessName = e.BusinessName, Status = e.Status,
        // 数据库操作：投影选择
        Columns = e.Columns?.Select(GenTableColumnDto.FromEntity).ToList() ?? []
    };
}

// 定义class GenTableSaveDto
// 定义类：GenTableSaveDto
public class GenTableSaveDto
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
    // 属性：TplCategory
    public string? TplCategory { get; set; } = "crud";
    // 属性：BusinessName
    public string? BusinessName { get; set; }
    // 属性：Status
    public string? Status { get; set; } = "0";
    // 属性 Columns
    // 属性：Columns
    public List<GenTableColumnSaveDto> Columns { get; set; } = [];
}

// 定义class DbTableInfo
// 定义类：DbTableInfo
public class DbTableInfo
{
    // 属性 TableName
    // 属性：TableName
    public string TableName { get; set; } = string.Empty;
    // 属性：TableComment
    public string? TableComment { get; set; }
    // 属性：CreateTime
    public string? CreateTime { get; set; }
}

// 定义class ColumnInfo
// 定义类：ColumnInfo
public class ColumnInfo
{
    // 属性 ColumnName
    // 属性：ColumnName
    public string ColumnName { get; set; } = string.Empty;
    // 属性：ColumnComment
    public string? ColumnComment { get; set; }
    // 属性：ColumnType
    public string? ColumnType { get; set; }
    // 属性：NetType
    public string? NetType { get; set; }
    // 属性：IsNullable
    public string? IsNullable { get; set; }
    // 属性：MaxLength
    public int? MaxLength { get; set; }
    // 属性：IsPk
    public string? IsPk { get; set; } = "0";
}
