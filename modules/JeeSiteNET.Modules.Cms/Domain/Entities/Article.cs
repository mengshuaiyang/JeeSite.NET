using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Cms.Domain.Entities;

public class Article : DataEntity, ICorpEntity
{
    public string ArticleCode { get; set; } = string.Empty;
    public string CategoryCode { get; set; } = string.Empty;
    public string? ModuleType { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string? Summary { get; set; }
    public string? Color { get; set; }
    public string? Image { get; set; }
    public string? Keywords { get; set; }
    public string? Description { get; set; }
    public string? Author { get; set; }
    public string? Source { get; set; }
    public string? Copyfrom { get; set; }
    public string? Tags { get; set; }
    public string? IsTop { get; set; } = "0";
    public string? IsRecommend { get; set; } = "0";
    public string? IsHot { get; set; } = "0";
    public decimal? Weight { get; set; }
    public DateTime? WeightDate { get; set; }
    public long? ClickCount { get; set; } = 0;
    public long? HitsPlus { get; set; }
    public long? HitsMinus { get; set; }
    public long? WordCount { get; set; }
    public DateTime? PublishDate { get; set; }
    public string? CustomContentView { get; set; }
    public string? ViewConfig { get; set; }

    public string? CorpCode { get; set; }
    public string? CorpName { get; set; }

    public ArticleData? ArticleData { get; set; }
    public ICollection<ArticlePosId>? PosIds { get; set; }
    public ICollection<ArticleTag>? ArticleTags { get; set; }
}
