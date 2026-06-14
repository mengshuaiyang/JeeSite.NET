// 定义 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
namespace JeeSiteNET.Modules.Cms.Domain.Entities;

// 定义class ArticlePosId
// 定义类：ArticlePosId
public class ArticlePosId
{
    // 属性 ArticleCode
    // 属性：ArticleCode
    public string ArticleCode { get; set; } = string.Empty;
    // 属性 PosId
    // 属性：PosId
    public string PosId { get; set; } = string.Empty;

    // 属性：Article
    public Article? Article { get; set; }
}
