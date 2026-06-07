namespace JeeSiteNET.Modules.Cms.Domain.Entities;

public class ArticlePosId
{
    public string ArticleCode { get; set; } = string.Empty;
    public string PosId { get; set; } = string.Empty;

    public Article? Article { get; set; }
}
