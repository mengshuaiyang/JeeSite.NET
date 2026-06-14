    // 引入 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
using JeeSiteNET.Modules.Cms.Domain.Entities;

// 定义 JeeSiteNET.Modules.Cms.Application.DTOs 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Application.DTOs
namespace JeeSiteNET.Modules.Cms.Application.DTOs;

// 定义class SiteDto
// 定义类：SiteDto
public class SiteDto
{
    // 属性 SiteCode
    // 属性：SiteCode
    public string SiteCode { get; set; } = string.Empty;
    // 属性 SiteName
    // 属性：SiteName
    public string SiteName { get; set; } = string.Empty;
    // 属性：Domain
    public string? Domain { get; set; }
    // 属性：Logo
    public string? Logo { get; set; }
    // 属性：Keywords
    public string? Keywords { get; set; }
    // 属性：Description
    public string? Description { get; set; }
    // 属性：Status
    public string? Status { get; set; }

    // 方法 FromEntity
    // 方法：FromEntity
    public static SiteDto FromEntity(Site e) => new()
    {
        SiteCode = e.SiteCode, SiteName = e.SiteName, Domain = e.Domain,
        Logo = e.Logo, Keywords = e.Keywords, Description = e.Description,
        Status = e.Status
    };
}

// 定义class SiteSaveDto
// 定义类：SiteSaveDto
public class SiteSaveDto
{
    // 属性：SiteCode
    public string? SiteCode { get; set; }
    // 属性 SiteName
    // 属性：SiteName
    public string SiteName { get; set; } = string.Empty;
    // 属性：Domain
    public string? Domain { get; set; }
    // 属性：Logo
    public string? Logo { get; set; }
    // 属性：Keywords
    public string? Keywords { get; set; }
    // 属性：Description
    public string? Description { get; set; }
}
