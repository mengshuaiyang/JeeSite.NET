    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
namespace JeeSiteNET.Modules.Sys.Application.DTOs;

// 定义class BizCategoryDto
// 定义类：BizCategoryDto
public class BizCategoryDto
{
    // 属性 CategoryCode
    // 属性：CategoryCode
    public string CategoryCode { get; set; } = string.Empty;
    // 属性：ViewCode
    public string? ViewCode { get; set; }
    // 属性 CategoryName
    // 属性：CategoryName
    public string CategoryName { get; set; } = string.Empty;
    // 属性：ParentCode
    public string? ParentCode { get; set; }
    // 属性：ParentCodes
    public string? ParentCodes { get; set; }
    // 属性 TreeSort
    // 属性：TreeSort
    public decimal TreeSort { get; set; }
    // 属性：TreeSorts
    public string? TreeSorts { get; set; }
    // 属性：TreeLeaf
    public string? TreeLeaf { get; set; }
    // 属性 TreeLevel
    // 属性：TreeLevel
    public decimal TreeLevel { get; set; }
    // 属性：TreeNames
    public string? TreeNames { get; set; }
    // 属性：Status
    public string? Status { get; set; }
    // 属性：Children
    public List<BizCategoryDto>? Children { get; set; }

    // 方法 FromEntity
    // 方法：FromEntity
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
