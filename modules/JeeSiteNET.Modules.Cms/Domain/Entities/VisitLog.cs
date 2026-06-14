    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
namespace JeeSiteNET.Modules.Cms.Domain.Entities;

// 定义class VisitLog
// 定义类：VisitLog
public class VisitLog : BaseEntity, ICorpEntity
{
    // 属性 VisitId
    // 属性：VisitId
    public string VisitId { get; set; } = string.Empty;
    // 属性：RequestUrl
    public string? RequestUrl { get; set; }
    // 属性：RequestUrlHost
    public string? RequestUrlHost { get; set; }
    // 属性：SourceReferer
    public string? SourceReferer { get; set; }
    // 属性：SourceRefererHost
    public string? SourceRefererHost { get; set; }
    // 属性：SourceType
    public string? SourceType { get; set; }
    // 属性：SearchEngine
    public string? SearchEngine { get; set; }
    // 属性：SearchWord
    public string? SearchWord { get; set; }
    // 属性：RemoteAddr
    public string? RemoteAddr { get; set; }
    // 属性：UserAgent
    public string? UserAgent { get; set; }
    // 属性：UserLanguage
    public string? UserLanguage { get; set; }
    // 属性：UserScreenSize
    public string? UserScreenSize { get; set; }
    // 属性：UserDevice
    public string? UserDevice { get; set; }
    // 属性：UserOsName
    public string? UserOsName { get; set; }
    // 属性：UserBrowser
    public string? UserBrowser { get; set; }
    // 属性：UserBrowserVersion
    public string? UserBrowserVersion { get; set; }
    // 属性：UniqueVisitId
    public string? UniqueVisitId { get; set; }
    // 属性：VisitDate
    public string? VisitDate { get; set; }
    // 属性：VisitTime
    public DateTime? VisitTime { get; set; }
    // 属性：IsNewVisit
    public string? IsNewVisit { get; set; }
    // 属性：FirstVisitTime
    public long? FirstVisitTime { get; set; }
    // 属性：PrevRemainTime
    public long? PrevRemainTime { get; set; }
    // 属性：TotalRemainTime
    public long? TotalRemainTime { get; set; }
    // 属性：SiteCode
    public string? SiteCode { get; set; }
    // 属性：SiteName
    public string? SiteName { get; set; }
    // 属性：CategoryCode
    public string? CategoryCode { get; set; }
    // 属性：CategoryName
    public string? CategoryName { get; set; }
    // 属性：ContentId
    public string? ContentId { get; set; }
    // 属性：ContentTitle
    public string? ContentTitle { get; set; }
    // 属性：VisitUserCode
    public string? VisitUserCode { get; set; }
    // 属性：VisitUserName
    public string? VisitUserName { get; set; }

    // 属性：CorpCode
    public string? CorpCode { get; set; }
    // 属性：CorpName
    public string? CorpName { get; set; }
}
