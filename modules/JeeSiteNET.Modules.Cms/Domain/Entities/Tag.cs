// 定义 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
namespace JeeSiteNET.Modules.Cms.Domain.Entities;

// 定义class Tag
// 定义类：Tag
public class Tag
{
    // 属性 TagName
    // 属性：TagName
    public string TagName { get; set; } = string.Empty;
    // 属性 ClickNum
    // 属性：ClickNum
    public long ClickNum { get; set; }

    // 属性：ArticleTags
    public ICollection<ArticleTag>? ArticleTags { get; set; }
}
