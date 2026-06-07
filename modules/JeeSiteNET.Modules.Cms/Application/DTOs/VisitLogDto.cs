using JeeSiteNET.Modules.Cms.Domain.Entities;

namespace JeeSiteNET.Modules.Cms.Application.DTOs;

public class VisitLogDto
{
    public string VisitId { get; set; } = string.Empty;
    public string? RequestUrl { get; set; }
    public string? RemoteAddr { get; set; }
    public string? UserAgent { get; set; }
    public string? VisitDate { get; set; }
    public DateTime? VisitTime { get; set; }
    public string? IsNewVisit { get; set; }
    public string? SiteCode { get; set; }
    public string? CategoryCode { get; set; }
    public string? ContentId { get; set; }
    public string? VisitUserCode { get; set; }

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
