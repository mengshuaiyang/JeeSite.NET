    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
namespace JeeSiteNET.Modules.Cms.Domain.Entities;

// 定义class Comment
// 定义类：Comment
public class Comment : DataEntity, ICorpEntity
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

    // 属性：CorpCode
    public string? CorpCode { get; set; }
    // 属性：CorpName
    public string? CorpName { get; set; }
}
