    // 引入 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
using JeeSiteNET.Modules.Cms.Domain.Entities;

// 定义 JeeSiteNET.Modules.Cms.Application.DTOs 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Application.DTOs
namespace JeeSiteNET.Modules.Cms.Application.DTOs;

// 定义class ReportDto
// 定义类：ReportDto
public class ReportDto
{
    // 属性 ReportCode
    // 属性：ReportCode
    public string ReportCode { get; set; } = string.Empty;
    // 属性 ArticleCode
    // 属性：ArticleCode
    public string ArticleCode { get; set; } = string.Empty;
    // 属性 ArticleTitle
    // 属性：ArticleTitle
    public string ArticleTitle { get; set; } = string.Empty;
    // 属性：ReportType
    public string? ReportType { get; set; }
    // 属性 Content
    // 属性：Content
    public string Content { get; set; } = string.Empty;
    // 属性：Ip
    public string? Ip { get; set; }
    // 属性：DealUserCode
    public string? DealUserCode { get; set; }
    // 属性：DealDate
    public DateTime? DealDate { get; set; }
    // 属性：DealResult
    public string? DealResult { get; set; }
    // 属性：Status
    public string? Status { get; set; }
    // 属性：CreateBy
    public string? CreateBy { get; set; }
    // 属性：CreateDate
    public DateTime? CreateDate { get; set; }

    // 方法 FromEntity
    // 方法：FromEntity
    public static ReportDto FromEntity(Report e) => new()
    {
        ReportCode = e.ReportCode, ArticleCode = e.ArticleCode, ArticleTitle = e.ArticleTitle,
        ReportType = e.ReportType, Content = e.Content, Ip = e.Ip,
        DealUserCode = e.DealUserCode, DealDate = e.DealDate, DealResult = e.DealResult,
        Status = e.Status, CreateBy = e.CreateBy, CreateDate = e.CreateDate
    };
}

// 定义class ReportSaveDto
// 定义类：ReportSaveDto
public class ReportSaveDto
{
    // 属性 ArticleCode
    // 属性：ArticleCode
    public string ArticleCode { get; set; } = string.Empty;
    // 属性 ArticleTitle
    // 属性：ArticleTitle
    public string ArticleTitle { get; set; } = string.Empty;
    // 属性：ReportType
    public string? ReportType { get; set; }
    // 属性 Content
    // 属性：Content
    public string Content { get; set; } = string.Empty;
}
