// 定义 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
namespace JeeSiteNET.Modules.Cms.Domain.Entities;

// 定义class ArticleTag
// 定义类：ArticleTag
public class ArticleTag
{
    // 属性 ArticleCode
    // 属性：ArticleCode
    public string ArticleCode { get; set; } = string.Empty;
    // 属性 TagName
    // 属性：TagName
    public string TagName { get; set; } = string.Empty;

    // 属性：Article
    public Article? Article { get; set; }
    // 属性：Tag
    public Tag? Tag { get; set; }
}
