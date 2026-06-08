using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Application.DTOs;

public class CompanyDto
{
    public string CompanyCode { get; set; } = string.Empty;
    public string? ViewCode { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string? AreaCode { get; set; }
    public string? AreaName { get; set; }
    public string? ParentCode { get; set; }
    public string? ParentCodes { get; set; }
    public string? TreeNames { get; set; }
    public string? TreeLeaf { get; set; }
    public string Status { get; set; } = "0";
    public string? Remarks { get; set; }
    public List<string> OfficeCodes { get; set; } = new();
}

public class CompanySaveDto
{
    public string? CompanyCode { get; set; }
    public string? ViewCode { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string? AreaCode { get; set; }
    public string? ParentCode { get; set; }
    public string? Remarks { get; set; }
    public List<string> OfficeCodes { get; set; } = new();
}

public class AreaDto
{
    public string AreaCode { get; set; } = string.Empty;
    public string AreaName { get; set; } = string.Empty;
    public string? AreaType { get; set; }
    public string? ParentCode { get; set; }
    public string? ParentCodes { get; set; }
    public decimal? TreeSort { get; set; }
    public string? TreeSorts { get; set; }
    public string? TreeNames { get; set; }
    public string? TreeLeaf { get; set; }
    public string Status { get; set; } = "0";
    public List<AreaDto> Children { get; set; } = new();
}

public static class CompanyMapping
{
    public static CompanyDto ToDto(this Company c, List<string> officeCodes) => new()
    {
        CompanyCode = c.CompanyCode,
        ViewCode = c.ViewCode,
        CompanyName = c.CompanyName,
        FullName = c.FullName,
        AreaCode = c.AreaCode,
        AreaName = c.AreaName,
        ParentCode = c.ParentCode,
        ParentCodes = c.ParentCodes,
        TreeNames = c.TreeNames,
        TreeLeaf = c.TreeLeaf,
        Status = c.Status ?? "0",
        Remarks = c.Remarks,
        OfficeCodes = officeCodes
    };

    public static Company ToEntity(this CompanySaveDto dto)
    {
        var now = DateTime.Now;
        return new Company
        {
            CompanyCode = dto.CompanyCode ?? IdGenerator.NewId(),
            ViewCode = dto.ViewCode,
            CompanyName = dto.CompanyName,
            FullName = dto.FullName,
            AreaCode = dto.AreaCode,
            ParentCode = dto.ParentCode ?? "0",
            Remarks = dto.Remarks,
            Status = "0",
            TreeLeaf = "1",
            CreateDate = now,
            UpdateDate = now
        };
    }

    public static AreaDto ToDto(this Area a) => new()
    {
        AreaCode = a.AreaCode,
        AreaName = a.AreaName,
        AreaType = a.AreaType,
        ParentCode = a.ParentCode,
        ParentCodes = a.ParentCodes,
        TreeSort = a.TreeSort,
        TreeSorts = a.TreeSorts,
        TreeNames = a.TreeNames,
        TreeLeaf = a.TreeLeaf,
        Status = a.Status ?? "0"
    };
}
