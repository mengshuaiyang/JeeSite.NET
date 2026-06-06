using JeeSiteNET.Modules.CodeGen.Domain.Entities;

namespace JeeSiteNET.Modules.CodeGen.Application.DTOs;

public class GenTableColumnDto
{
    public string ColumnId { get; set; } = string.Empty;
    public string ColumnName { get; set; } = string.Empty;
    public int? ColumnSort { get; set; }
    public string? ColumnComment { get; set; }
    public string? ColumnType { get; set; }
    public string? NetType { get; set; }
    public string? PropertyName { get; set; }
    public int? MaxLength { get; set; }
    public string? IsPk { get; set; }
    public string? IsNullable { get; set; }
    public string? IsInsert { get; set; }
    public string? IsEdit { get; set; }
    public string? IsList { get; set; }
    public string? IsQuery { get; set; }
    public string? QueryType { get; set; }
    public string? HtmlType { get; set; }
    public string? DictType { get; set; }

    public static GenTableColumnDto FromEntity(GenTableColumn e) => new()
    {
        ColumnId = e.ColumnId, ColumnName = e.ColumnName, ColumnSort = e.ColumnSort,
        ColumnComment = e.ColumnComment, ColumnType = e.ColumnType, NetType = e.NetType,
        PropertyName = e.PropertyName, MaxLength = e.MaxLength, IsPk = e.IsPk,
        IsNullable = e.IsNullable, IsInsert = e.IsInsert, IsEdit = e.IsEdit,
        IsList = e.IsList, IsQuery = e.IsQuery, QueryType = e.QueryType,
        HtmlType = e.HtmlType, DictType = e.DictType
    };
}

public class GenTableColumnSaveDto
{
    public string ColumnName { get; set; } = string.Empty;
    public string? ColumnComment { get; set; }
    public string? NetType { get; set; }
    public string? PropertyName { get; set; }
    public int? ColumnSort { get; set; }
    public string? IsPk { get; set; } = "0";
    public string? IsNullable { get; set; } = "1";
    public string? IsInsert { get; set; } = "1";
    public string? IsEdit { get; set; } = "1";
    public string? IsList { get; set; } = "1";
    public string? IsQuery { get; set; } = "0";
    public string? QueryType { get; set; } = "EQ";
    public string? HtmlType { get; set; } = "input";
}
