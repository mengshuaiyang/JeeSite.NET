using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Cms.Domain.Entities;

public class Article : DataEntity
{
    public string ArticleCode { get; set; } = string.Empty;
    public string CategoryCode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string? Summary { get; set; }
    public string? Content { get; set; }
    public string? Author { get; set; }
    public string? Source { get; set; }
    public string? Image { get; set; }
    public string? Tags { get; set; }
    public string? IsTop { get; set; } = "0";
    public string? IsRecommend { get; set; } = "0";
    public string? IsHot { get; set; } = "0";
    public long? ClickCount { get; set; } = 0;
    public DateTime? PublishDate { get; set; }
}
