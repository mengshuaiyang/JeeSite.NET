    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
namespace JeeSiteNET.Modules.Cms.Domain.Entities;

// 定义class ArticleData
// 定义类：ArticleData
public class ArticleData : BaseEntity, IExtendEntity
{
    // 属性 ArticleCode
    // 属性：ArticleCode
    public string ArticleCode { get; set; } = string.Empty;
    // 属性：Content
    public string? Content { get; set; }
    // 属性：Relation
    public string? Relation { get; set; }
    // 属性：IsCanComment
    public string? IsCanComment { get; set; } = "1";

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

    // 属性：Article
    public Article? Article { get; set; }
}
