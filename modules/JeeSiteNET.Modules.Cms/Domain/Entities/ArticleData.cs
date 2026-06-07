using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Cms.Domain.Entities;

public class ArticleData : BaseEntity, IExtendEntity
{
    public string ArticleCode { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? Relation { get; set; }
    public string? IsCanComment { get; set; } = "1";

    public string? ExtendS1 { get; set; }
    public string? ExtendS2 { get; set; }
    public string? ExtendS3 { get; set; }
    public string? ExtendS4 { get; set; }
    public string? ExtendS5 { get; set; }
    public string? ExtendS6 { get; set; }
    public string? ExtendS7 { get; set; }
    public string? ExtendS8 { get; set; }
    public int? ExtendI1 { get; set; }
    public int? ExtendI2 { get; set; }
    public int? ExtendI3 { get; set; }
    public int? ExtendI4 { get; set; }
    public decimal? ExtendF1 { get; set; }
    public decimal? ExtendF2 { get; set; }
    public decimal? ExtendF3 { get; set; }
    public decimal? ExtendF4 { get; set; }
    public DateTime? ExtendD1 { get; set; }
    public DateTime? ExtendD2 { get; set; }
    public DateTime? ExtendD3 { get; set; }
    public DateTime? ExtendD4 { get; set; }
    public string? ExtendJson { get; set; }

    public Article? Article { get; set; }
}
