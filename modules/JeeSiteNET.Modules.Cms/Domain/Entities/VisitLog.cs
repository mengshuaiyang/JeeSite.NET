using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Cms.Domain.Entities;

public class VisitLog : BaseEntity, ICorpEntity
{
    public string VisitId { get; set; } = string.Empty;
    public string? RequestUrl { get; set; }
    public string? RequestUrlHost { get; set; }
    public string? SourceReferer { get; set; }
    public string? SourceRefererHost { get; set; }
    public string? SourceType { get; set; }
    public string? SearchEngine { get; set; }
    public string? SearchWord { get; set; }
    public string? RemoteAddr { get; set; }
    public string? UserAgent { get; set; }
    public string? UserLanguage { get; set; }
    public string? UserScreenSize { get; set; }
    public string? UserDevice { get; set; }
    public string? UserOsName { get; set; }
    public string? UserBrowser { get; set; }
    public string? UserBrowserVersion { get; set; }
    public string? UniqueVisitId { get; set; }
    public string? VisitDate { get; set; }
    public DateTime? VisitTime { get; set; }
    public string? IsNewVisit { get; set; }
    public long? FirstVisitTime { get; set; }
    public long? PrevRemainTime { get; set; }
    public long? TotalRemainTime { get; set; }
    public string? SiteCode { get; set; }
    public string? SiteName { get; set; }
    public string? CategoryCode { get; set; }
    public string? CategoryName { get; set; }
    public string? ContentId { get; set; }
    public string? ContentTitle { get; set; }
    public string? VisitUserCode { get; set; }
    public string? VisitUserName { get; set; }

    public string? CorpCode { get; set; }
    public string? CorpName { get; set; }
}
