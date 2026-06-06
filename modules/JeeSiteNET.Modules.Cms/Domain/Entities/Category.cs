using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Cms.Domain.Entities;

public class Category : TreeEntity
{
    public string CategoryCode { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string? CategoryType { get; set; } = "article";
    public string? Image { get; set; }
    public string? Link { get; set; }
    public string? Keywords { get; set; }
    public string? Description { get; set; }
    public string? IsShow { get; set; } = "1";
    public string? SiteCode { get; set; }

    public override string GetName() => CategoryName;
}
