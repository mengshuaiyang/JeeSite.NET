using JeeSiteNET.Modules.Cms.Domain.Entities;

namespace JeeSiteNET.Modules.Cms.Application.DTOs;

public class CategoryDto
{
    public string CategoryCode { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string? CategoryType { get; set; }
    public string? Image { get; set; }
    public string? Link { get; set; }
    public string? Keywords { get; set; }
    public string? Description { get; set; }
    public string? IsShow { get; set; }
    public string? SiteCode { get; set; }
    public string ParentCode { get; set; } = "0";
    public string? ParentCodes { get; set; }
    public decimal TreeSort { get; set; }
    public string? TreeLeaf { get; set; }
    public decimal TreeLevel { get; set; }
    public string? TreeNames { get; set; }
    public string? Status { get; set; }
    public List<CategoryDto> Children { get; set; } = [];

    public static CategoryDto FromEntity(Category e) => new()
    {
        CategoryCode = e.CategoryCode, CategoryName = e.CategoryName,
        CategoryType = e.CategoryType, Image = e.Image, Link = e.Link,
        Keywords = e.Keywords, Description = e.Description, IsShow = e.IsShow,
        SiteCode = e.SiteCode, ParentCode = e.ParentCode, ParentCodes = e.ParentCodes,
        TreeSort = e.TreeSort, TreeLeaf = e.TreeLeaf, TreeLevel = e.TreeLevel,
        TreeNames = e.TreeNames, Status = e.Status
    };
}

public class CategorySaveDto
{
    public string? CategoryCode { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? CategoryType { get; set; } = "article";
    public string? Image { get; set; }
    public string? Link { get; set; }
    public string? Keywords { get; set; }
    public string? Description { get; set; }
    public string? IsShow { get; set; } = "1";
    public string? SiteCode { get; set; }
    public string ParentCode { get; set; } = "0";
    public decimal TreeSort { get; set; } = 1000;
}
