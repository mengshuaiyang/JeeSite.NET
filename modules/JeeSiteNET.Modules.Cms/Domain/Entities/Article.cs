    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
namespace JeeSiteNET.Modules.Cms.Domain.Entities;

// 定义class Article
// 定义类：Article
public class Article : DataEntity, ICorpEntity
{
    // 属性 ArticleCode
    // 属性：ArticleCode
    public string ArticleCode { get; set; } = string.Empty;
    // 属性 CategoryCode
    // 属性：CategoryCode
    public string CategoryCode { get; set; } = string.Empty;
    // 属性：ModuleType
    public string? ModuleType { get; set; }
    // 属性 Title
    // 属性：Title
    public string Title { get; set; } = string.Empty;
    // 属性：Subtitle
    public string? Subtitle { get; set; }
    // 属性：Summary
    public string? Summary { get; set; }
    // 属性：Color
    public string? Color { get; set; }
    // 属性：Image
    public string? Image { get; set; }
    // 属性：Keywords
    public string? Keywords { get; set; }
    // 属性：Description
    public string? Description { get; set; }
    // 属性：Author
    public string? Author { get; set; }
    // 属性：Source
    public string? Source { get; set; }
    // 属性：Copyfrom
    public string? Copyfrom { get; set; }
    // 属性：Tags
    public string? Tags { get; set; }
    // 属性：IsTop
    public string? IsTop { get; set; } = "0";
    // 属性：IsRecommend
    public string? IsRecommend { get; set; } = "0";
    // 属性：IsHot
    public string? IsHot { get; set; } = "0";
    // 属性：Weight
    public decimal? Weight { get; set; }
    // 属性：WeightDate
    public DateTime? WeightDate { get; set; }
    // 属性：ClickCount
    public long? ClickCount { get; set; } = 0;
    // 属性：HitsPlus
    public long? HitsPlus { get; set; }
    // 属性：HitsMinus
    public long? HitsMinus { get; set; }
    // 属性：WordCount
    public long? WordCount { get; set; }
    // 属性：PublishDate
    public DateTime? PublishDate { get; set; }
    // 属性：CustomContentView
    public string? CustomContentView { get; set; }
    // 属性：ViewConfig
    public string? ViewConfig { get; set; }

    // 属性：CorpCode
    public string? CorpCode { get; set; }
    // 属性：CorpName
    public string? CorpName { get; set; }

    // 属性：ArticleData
    public ArticleData? ArticleData { get; set; }
    // 属性：PosIds
    public ICollection<ArticlePosId>? PosIds { get; set; }
    // 属性：ArticleTags
    public ICollection<ArticleTag>? ArticleTags { get; set; }
}
