    // 引入 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
using JeeSiteNET.Modules.Cms.Domain.Entities;

// 定义 JeeSiteNET.Modules.Cms.Application.DTOs 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Application.DTOs
namespace JeeSiteNET.Modules.Cms.Application.DTOs;

// 定义class CategoryDto
// 定义类：CategoryDto
public class CategoryDto
{
    // 属性 CategoryCode
    // 属性：CategoryCode
    public string CategoryCode { get; set; } = string.Empty;
    // 属性 CategoryName
    // 属性：CategoryName
    public string CategoryName { get; set; } = string.Empty;
    // 属性：CategoryType
    public string? CategoryType { get; set; }
    // 属性：Image
    public string? Image { get; set; }
    // 属性：Link
    public string? Link { get; set; }
    // 属性：Keywords
    public string? Keywords { get; set; }
    // 属性：Description
    public string? Description { get; set; }
    // 属性：IsShow
    public string? IsShow { get; set; }
    // 属性：SiteCode
    public string? SiteCode { get; set; }
    // 属性 ParentCode
    // 属性：ParentCode
    public string ParentCode { get; set; } = "0";
    // 属性：ParentCodes
    public string? ParentCodes { get; set; }
    // 属性 TreeSort
    // 属性：TreeSort
    public decimal TreeSort { get; set; }
    // 属性：TreeLeaf
    public string? TreeLeaf { get; set; }
    // 属性 TreeLevel
    // 属性：TreeLevel
    public decimal TreeLevel { get; set; }
    // 属性：TreeNames
    public string? TreeNames { get; set; }
    // 属性：Status
    public string? Status { get; set; }
    // 属性 Children
    // 属性：Children
    public List<CategoryDto> Children { get; set; } = [];

    // 方法 FromEntity
    // 方法：FromEntity
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

// 定义class CategorySaveDto
// 定义类：CategorySaveDto
public class CategorySaveDto
{
    // 属性：CategoryCode
    public string? CategoryCode { get; set; }
    // 属性 CategoryName
    // 属性：CategoryName
    public string CategoryName { get; set; } = string.Empty;
    // 属性：CategoryType
    public string? CategoryType { get; set; } = "article";
    // 属性：Image
    public string? Image { get; set; }
    // 属性：Link
    public string? Link { get; set; }
    // 属性：Keywords
    public string? Keywords { get; set; }
    // 属性：Description
    public string? Description { get; set; }
    // 属性：IsShow
    public string? IsShow { get; set; } = "1";
    // 属性：SiteCode
    public string? SiteCode { get; set; }
    // 属性 ParentCode
    // 属性：ParentCode
    public string ParentCode { get; set; } = "0";
    // 属性 TreeSort
    // 属性：TreeSort
    public decimal TreeSort { get; set; } = 1000;
}
