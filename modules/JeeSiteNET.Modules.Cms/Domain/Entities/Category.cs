    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
namespace JeeSiteNET.Modules.Cms.Domain.Entities;

// 定义class Category
// 定义类：Category
public class Category : TreeEntity, ICorpEntity, IExtendEntity
{
    // 属性 CategoryCode
    // 属性：CategoryCode
    public string CategoryCode { get; set; } = string.Empty;
    // 属性 CategoryName
    // 属性：CategoryName
    public string CategoryName { get; set; } = string.Empty;
    // 属性：CategoryType
    public string? CategoryType { get; set; } = "article";
    // 属性：Image
    public string? Image { get; set; }
    // 属性：Link
    public string? Link { get; set; }
    // 属性：Target
    public string? Target { get; set; }
    // 属性：Keywords
    public string? Keywords { get; set; }
    // 属性：Description
    public string? Description { get; set; }
    // 属性：InMenu
    public string? InMenu { get; set; } = "1";
    // 属性：InList
    public string? InList { get; set; } = "1";
    // 属性：ShowModes
    public string? ShowModes { get; set; }
    // 属性：IsShow
    public string? IsShow { get; set; } = "1";
    // 属性：IsNeedAudit
    public string? IsNeedAudit { get; set; } = "1";
    // 属性：IsCanComment
    public string? IsCanComment { get; set; } = "1";
    // 属性：SiteCode
    public string? SiteCode { get; set; }
    // 属性：CustomListView
    public string? CustomListView { get; set; }
    // 属性：CustomContentView
    public string? CustomContentView { get; set; }
    // 属性：ViewConfig
    public string? ViewConfig { get; set; }

    // 属性：CorpCode
    public string? CorpCode { get; set; }
    // 属性：CorpName
    public string? CorpName { get; set; }

    // 属性：ExtendS1
    public string? ExtendS1 { get; set; }
    // 属性：ExtendS2
    public string? ExtendS2 { get; set; }
    // 属性：ExtendS3
    public string? ExtendS3 { get; set; }
    // 属性：ExtendS4
    public string? ExtendS4 { get; set; }
    // 属性：ExtendS5
    public string? ExtendS5 { get; set; }
    // 属性：ExtendS6
    public string? ExtendS6 { get; set; }
    // 属性：ExtendS7
    public string? ExtendS7 { get; set; }
    // 属性：ExtendS8
    public string? ExtendS8 { get; set; }
    // 属性：ExtendI1
    public int? ExtendI1 { get; set; }
    // 属性：ExtendI2
    public int? ExtendI2 { get; set; }
    // 属性：ExtendI3
    public int? ExtendI3 { get; set; }
    // 属性：ExtendI4
    public int? ExtendI4 { get; set; }
    // 属性：ExtendF1
    public decimal? ExtendF1 { get; set; }
    // 属性：ExtendF2
    public decimal? ExtendF2 { get; set; }
    // 属性：ExtendF3
    public decimal? ExtendF3 { get; set; }
    // 属性：ExtendF4
    public decimal? ExtendF4 { get; set; }
    // 属性：ExtendD1
    public DateTime? ExtendD1 { get; set; }
    // 属性：ExtendD2
    public DateTime? ExtendD2 { get; set; }
    // 属性：ExtendD3
    public DateTime? ExtendD3 { get; set; }
    // 属性：ExtendD4
    public DateTime? ExtendD4 { get; set; }
    // 属性：ExtendJson
    public string? ExtendJson { get; set; }

    // 方法：GetName
    public override string GetName() => CategoryName;
}
