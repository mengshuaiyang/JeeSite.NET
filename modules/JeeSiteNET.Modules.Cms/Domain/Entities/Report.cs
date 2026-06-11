using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Cms.Domain.Entities;

public class Report : DataEntity, ICorpEntity
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
    public string? CorpCode { get; set; }
    public string? CorpName { get; set; }
}
