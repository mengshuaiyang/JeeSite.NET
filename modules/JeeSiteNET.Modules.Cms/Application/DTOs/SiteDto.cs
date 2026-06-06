using JeeSiteNET.Modules.Cms.Domain.Entities;

namespace JeeSiteNET.Modules.Cms.Application.DTOs;

public class SiteDto
{
    public string SiteCode { get; set; } = string.Empty;
    public string SiteName { get; set; } = string.Empty;
    public string? Domain { get; set; }
    public string? Logo { get; set; }
    public string? Keywords { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }

    public static SiteDto FromEntity(Site e) => new()
    {
        SiteCode = e.SiteCode, SiteName = e.SiteName, Domain = e.Domain,
        Logo = e.Logo, Keywords = e.Keywords, Description = e.Description,
        Status = e.Status
    };
}

public class SiteSaveDto
{
    public string? SiteCode { get; set; }
    public string SiteName { get; set; } = string.Empty;
    public string? Domain { get; set; }
    public string? Logo { get; set; }
    public string? Keywords { get; set; }
    public string? Description { get; set; }
}
