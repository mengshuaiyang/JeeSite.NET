    // 引入 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
using JeeSiteNET.Modules.Cms.Domain.Entities;

// 定义 JeeSiteNET.Modules.Cms.Application.DTOs 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Application.DTOs
namespace JeeSiteNET.Modules.Cms.Application.DTOs;

// 定义class VisitLogDto
// 定义类：VisitLogDto
public class VisitLogDto
{
    // 属性 VisitId
    // 属性：VisitId
    public string VisitId { get; set; } = string.Empty;
    // 属性：RequestUrl
    public string? RequestUrl { get; set; }
    // 属性：RemoteAddr
    public string? RemoteAddr { get; set; }
    // 属性：UserAgent
    public string? UserAgent { get; set; }
    // 属性：VisitDate
    public string? VisitDate { get; set; }
    // 属性：VisitTime
    public DateTime? VisitTime { get; set; }
    // 属性：IsNewVisit
    public string? IsNewVisit { get; set; }
    // 属性：SiteCode
    public string? SiteCode { get; set; }
    // 属性：CategoryCode
    public string? CategoryCode { get; set; }
    // 属性：ContentId
    public string? ContentId { get; set; }
    // 属性：VisitUserCode
    public string? VisitUserCode { get; set; }

    // 方法 FromEntity
    // 方法：FromEntity
    public static VisitLogDto FromEntity(VisitLog e) => new()
    {
        VisitId = e.VisitId, RequestUrl = e.RequestUrl,
        RemoteAddr = e.RemoteAddr, UserAgent = e.UserAgent,
        VisitDate = e.VisitDate, VisitTime = e.VisitTime,
        IsNewVisit = e.IsNewVisit, SiteCode = e.SiteCode,
        CategoryCode = e.CategoryCode, ContentId = e.ContentId,
        VisitUserCode = e.VisitUserCode
    };
}
