namespace JeeSiteNET.Modules.Cms.Domain.Entities;

public class ArticleTag
{
    public string ArticleCode { get; set; } = string.Empty;
    public string TagName { get; set; } = string.Empty;

    public Article? Article { get; set; }
    public Tag? Tag { get; set; }
}
