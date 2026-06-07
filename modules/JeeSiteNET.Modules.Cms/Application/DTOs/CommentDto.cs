using JeeSiteNET.Modules.Cms.Domain.Entities;

namespace JeeSiteNET.Modules.Cms.Application.DTOs;

public class CommentDto
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
    public string? Status { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? CreateDate { get; set; }

    public static CommentDto FromEntity(Comment e) => new()
    {
        CommentCode = e.CommentCode, CategoryCode = e.CategoryCode,
        ArticleCode = e.ArticleCode, ParentCode = e.ParentCode,
        ArticleTitle = e.ArticleTitle, Content = e.Content,
        Name = e.Name, Ip = e.Ip, AuditUserCode = e.AuditUserCode,
        AuditDate = e.AuditDate, AuditComment = e.AuditComment,
        HitsPlus = e.HitsPlus, HitsMinus = e.HitsMinus,
        Status = e.Status, CreateBy = e.CreateBy, CreateDate = e.CreateDate
    };
}

public class CommentSaveDto
{
    public string? CommentCode { get; set; }
    public string CategoryCode { get; set; } = string.Empty;
    public string ArticleCode { get; set; } = string.Empty;
    public string? ParentCode { get; set; }
    public string ArticleTitle { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Name { get; set; }
}
