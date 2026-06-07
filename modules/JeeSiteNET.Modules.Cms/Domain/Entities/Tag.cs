namespace JeeSiteNET.Modules.Cms.Domain.Entities;

public class Tag
{
    public string TagName { get; set; } = string.Empty;
    public long ClickNum { get; set; }

    public ICollection<ArticleTag>? ArticleTags { get; set; }
}
