    // 引入 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
using JeeSiteNET.Modules.Cms.Domain.Entities;

// 定义 JeeSiteNET.Modules.Cms.Application.DTOs 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Application.DTOs
namespace JeeSiteNET.Modules.Cms.Application.DTOs;

// 定义class CommentDto
// 定义类：CommentDto
public class CommentDto
{
    // 属性 CommentCode
    // 属性：CommentCode
    public string CommentCode { get; set; } = string.Empty;
    // 属性 CategoryCode
    // 属性：CategoryCode
    public string CategoryCode { get; set; } = string.Empty;
    // 属性 ArticleCode
    // 属性：ArticleCode
    public string ArticleCode { get; set; } = string.Empty;
    // 属性：ParentCode
    public string? ParentCode { get; set; }
    // 属性 ArticleTitle
    // 属性：ArticleTitle
    public string ArticleTitle { get; set; } = string.Empty;
    // 属性 Content
    // 属性：Content
    public string Content { get; set; } = string.Empty;
    // 属性：Name
    public string? Name { get; set; }
    // 属性：Ip
    public string? Ip { get; set; }
    // 属性：AuditUserCode
    public string? AuditUserCode { get; set; }
    // 属性：AuditDate
    public DateTime? AuditDate { get; set; }
    // 属性：AuditComment
    public string? AuditComment { get; set; }
    // 属性：HitsPlus
    public long? HitsPlus { get; set; }
    // 属性：HitsMinus
    public long? HitsMinus { get; set; }
    // 属性：Status
    public string? Status { get; set; }
    // 属性：CreateBy
    public string? CreateBy { get; set; }
    // 属性：CreateDate
    public DateTime? CreateDate { get; set; }

    // 方法 FromEntity
    // 方法：FromEntity
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

// 定义class CommentSaveDto
// 定义类：CommentSaveDto
public class CommentSaveDto
{
    // 属性：CommentCode
    public string? CommentCode { get; set; }
    // 属性 CategoryCode
    // 属性：CategoryCode
    public string CategoryCode { get; set; } = string.Empty;
    // 属性 ArticleCode
    // 属性：ArticleCode
    public string ArticleCode { get; set; } = string.Empty;
    // 属性：ParentCode
    public string? ParentCode { get; set; }
    // 属性 ArticleTitle
    // 属性：ArticleTitle
    public string ArticleTitle { get; set; } = string.Empty;
    // 属性 Content
    // 属性：Content
    public string Content { get; set; } = string.Empty;
    // 属性：Name
    public string? Name { get; set; }
}
