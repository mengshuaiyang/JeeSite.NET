using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Cms.Domain.Entities;

public class Site : DataEntity
{
    public string SiteCode { get; set; } = string.Empty;
    public string SiteName { get; set; } = string.Empty;
    public string? Domain { get; set; }
    public string? Logo { get; set; }
    public string? Keywords { get; set; }
    public string? Description { get; set; }
}
