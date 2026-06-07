using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Cms.Domain.Entities;

public class Comment : DataEntity, ICorpEntity
{
    public string CommentCode { get; set; } = string.Empty;
    public string CategoryCode { get; set; } = string.Empty;
    public string ArticleCode { get; set; } = string.Empty;
    public string? ParentCode { get; set; }
    public string ArticleTitle { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string? Ip { get; set; }
    public string? AuditUserCode { get; set; }
    public DateTime? AuditDate { get; set; }
    public string? AuditComment { get; set; }
    public long? HitsPlus { get; set; }
    public long? HitsMinus { get; set; }

    public string? CorpCode { get; set; }
    public string? CorpName { get; set; }
}
