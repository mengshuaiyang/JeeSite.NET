    // 引入 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
using JeeSiteNET.Modules.Cms.Domain.Entities;

// 定义 JeeSiteNET.Modules.Cms.Application.DTOs 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Application.DTOs
namespace JeeSiteNET.Modules.Cms.Application.DTOs;

// 定义class TagDto
// 定义类：TagDto
public class TagDto
{
    // 属性 TagName
    // 属性：TagName
    public string TagName { get; set; } = string.Empty;
    // 属性 ClickNum
    // 属性：ClickNum
    public long ClickNum { get; set; }
    // 属性 ArticleCount
    // 属性：ArticleCount
    public long ArticleCount { get; set; }

    // 方法 FromEntity
    // 方法：FromEntity
    public static TagDto FromEntity(Tag e) => new() { TagName = e.TagName, ClickNum = e.ClickNum };
}
