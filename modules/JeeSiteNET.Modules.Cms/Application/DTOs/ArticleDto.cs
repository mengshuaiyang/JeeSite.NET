using JeeSiteNET.Modules.Cms.Domain.Entities;

namespace JeeSiteNET.Modules.Cms.Application.DTOs;

public class ArticleDto
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
    public string? IsTop { get; set; }
    public string? IsRecommend { get; set; }
    public string? IsHot { get; set; }
    public long? ClickCount { get; set; }
    public DateTime? PublishDate { get; set; }
    public string? CategoryName { get; set; }
    public string? Status { get; set; }

    public static ArticleDto FromEntity(Article e, string? categoryName = null) => new()
    {
        ArticleCode = e.ArticleCode, CategoryCode = e.CategoryCode,
        Title = e.Title, Subtitle = e.Subtitle, Summary = e.Summary,
        Content = e.Content, Author = e.Author, Source = e.Source,
        Image = e.Image, Tags = e.Tags, IsTop = e.IsTop,
        IsRecommend = e.IsRecommend, IsHot = e.IsHot,
        ClickCount = e.ClickCount, PublishDate = e.PublishDate,
        CategoryName = categoryName, Status = e.Status
    };
}

public class ArticleSaveDto
{
    public string? ArticleCode { get; set; }
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
    public DateTime? PublishDate { get; set; }
}
