using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Cms.Domain.Entities;

public class Category : TreeEntity, ICorpEntity, IExtendEntity
{
    public string CategoryCode { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string? CategoryType { get; set; } = "article";
    public string? Image { get; set; }
    public string? Link { get; set; }
    public string? Target { get; set; }
    public string? Keywords { get; set; }
    public string? Description { get; set; }
    public string? InMenu { get; set; } = "1";
    public string? InList { get; set; } = "1";
    public string? ShowModes { get; set; }
    public string? IsShow { get; set; } = "1";
    public string? IsNeedAudit { get; set; } = "1";
    public string? IsCanComment { get; set; } = "1";
    public string? SiteCode { get; set; }
    public string? CustomListView { get; set; }
    public string? CustomContentView { get; set; }
    public string? ViewConfig { get; set; }

    public string? CorpCode { get; set; }
    public string? CorpName { get; set; }

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

    public override string GetName() => CategoryName;
}
