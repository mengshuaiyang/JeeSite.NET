    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
namespace JeeSiteNET.Modules.Cms.Domain.Entities;

// 定义class Site
// 定义类：Site
public class Site : DataEntity, ICorpEntity
{
    // 属性 SiteCode
    // 属性：SiteCode
    public string SiteCode { get; set; } = string.Empty;
    // 属性 SiteName
    // 属性：SiteName
    public string SiteName { get; set; } = string.Empty;
    // 属性：SiteSort
    public int? SiteSort { get; set; }
    // 属性：Title
    public string? Title { get; set; }
    // 属性：Logo
    public string? Logo { get; set; }
    // 属性：Domain
    public string? Domain { get; set; }
    // 属性：Keywords
    public string? Keywords { get; set; }
    // 属性：Description
    public string? Description { get; set; }
    // 属性：Theme
    public string? Theme { get; set; }
    // 属性：Copyright
    public string? Copyright { get; set; }
    // 属性：CustomIndexView
    public string? CustomIndexView { get; set; }

    // 属性：CorpCode
    public string? CorpCode { get; set; }
    // 属性：CorpName
    public string? CorpName { get; set; }
}
