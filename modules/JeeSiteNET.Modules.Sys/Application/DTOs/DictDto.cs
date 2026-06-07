namespace JeeSiteNET.Modules.Sys.Application.DTOs;

public class DictTypeDto
{
    public string DictTypeCode { get; set; } = string.Empty;
    public string DictName { get; set; } = string.Empty;
    public string? IsSys { get; set; }
    public decimal? Sort { get; set; }
    public string? Status { get; set; }
}

public class DictTypeSaveDto
{
    public string? DictTypeCode { get; set; }
    public string DictName { get; set; } = string.Empty;
    public string? IsSys { get; set; }
    public decimal? Sort { get; set; }
}

public class DictDataDto
{
    public string DictCode { get; set; } = string.Empty;
    public string DictType { get; set; } = string.Empty;
    public string DictLabel { get; set; } = string.Empty;
    public string DictValue { get; set; } = string.Empty;
    public decimal? Sort { get; set; }
    public string? ParentCode { get; set; }
    public string? TreeLeaf { get; set; }
    public string? Status { get; set; }
    public List<DictDataDto> Children { get; set; } = new();
}

public class DictDataSaveDto
{
    public string? DictCode { get; set; }
    public string DictType { get; set; } = string.Empty;
    public string DictLabel { get; set; } = string.Empty;
    public string DictValue { get; set; } = string.Empty;
    public string? ParentCode { get; set; }
    public decimal? Sort { get; set; }
}
