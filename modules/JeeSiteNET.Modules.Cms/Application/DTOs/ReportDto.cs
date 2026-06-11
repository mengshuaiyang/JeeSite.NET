using JeeSiteNET.Modules.Cms.Domain.Entities;

namespace JeeSiteNET.Modules.Cms.Application.DTOs;

public class ReportDto
{
    public string ReportCode { get; set; } = string.Empty;
    public string ArticleCode { get; set; } = string.Empty;
    public string ArticleTitle { get; set; } = string.Empty;
    public string? ReportType { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? Ip { get; set; }
    public string? DealUserCode { get; set; }
    public DateTime? DealDate { get; set; }
    public string? DealResult { get; set; }
    public string? Status { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? CreateDate { get; set; }

    public static ReportDto FromEntity(Report e) => new()
    {
        ReportCode = e.ReportCode, ArticleCode = e.ArticleCode, ArticleTitle = e.ArticleTitle,
        ReportType = e.ReportType, Content = e.Content, Ip = e.Ip,
        DealUserCode = e.DealUserCode, DealDate = e.DealDate, DealResult = e.DealResult,
        Status = e.Status, CreateBy = e.CreateBy, CreateDate = e.CreateDate
    };
}

public class ReportSaveDto
{
    public string ArticleCode { get; set; } = string.Empty;
    public string ArticleTitle { get; set; } = string.Empty;
    public string? ReportType { get; set; }
    public string Content { get; set; } = string.Empty;
}
