using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Cms.Domain.Entities;

public class Site : DataEntity, ICorpEntity
{
    public string SiteCode { get; set; } = string.Empty;
    public string SiteName { get; set; } = string.Empty;
    public int? SiteSort { get; set; }
    public string? Title { get; set; }
    public string? Logo { get; set; }
    public string? Domain { get; set; }
    public string? Keywords { get; set; }
    public string? Description { get; set; }
    public string? Theme { get; set; }
    public string? Copyright { get; set; }
    public string? CustomIndexView { get; set; }

    public string? CorpCode { get; set; }
    public string? CorpName { get; set; }
}
