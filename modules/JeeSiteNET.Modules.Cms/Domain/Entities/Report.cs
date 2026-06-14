    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
namespace JeeSiteNET.Modules.Cms.Domain.Entities;

// 定义class Report
// 定义类：Report
public class Report : DataEntity, ICorpEntity
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
    // 属性：CorpCode
    public string? CorpCode { get; set; }
    // 属性：CorpName
    public string? CorpName { get; set; }
}
