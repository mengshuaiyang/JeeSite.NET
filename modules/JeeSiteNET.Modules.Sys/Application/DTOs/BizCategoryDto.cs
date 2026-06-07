using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Application.DTOs;

public class BizCategoryDto
{
    public string CategoryCode { get; set; } = string.Empty;
    public string? ViewCode { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? ParentCode { get; set; }
    public string? ParentCodes { get; set; }
    public decimal TreeSort { get; set; }
    public string? TreeSorts { get; set; }
    public string? TreeLeaf { get; set; }
    public decimal TreeLevel { get; set; }
    public string? TreeNames { get; set; }
    public string? Status { get; set; }
    public List<BizCategoryDto>? Children { get; set; }

    public static BizCategoryDto FromEntity(BizCategory e) => new()
    {
        CategoryCode = e.CategoryCode, ViewCode = e.ViewCode,
        CategoryName = e.CategoryName, ParentCode = e.ParentCode,
        ParentCodes = e.ParentCodes, TreeSort = e.TreeSort,
        TreeSorts = e.TreeSorts, TreeLeaf = e.TreeLeaf,
        TreeLevel = e.TreeLevel, TreeNames = e.TreeNames,
        Status = e.Status
    };
}
