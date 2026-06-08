using JeeSiteNET.Modules.Cms.Domain.Entities;

namespace JeeSiteNET.Modules.Cms.Application.DTOs;

public class TagDto
{
    public string TagName { get; set; } = string.Empty;
    public long ClickNum { get; set; }
    public long ArticleCount { get; set; }

    public static TagDto FromEntity(Tag e) => new() { TagName = e.TagName, ClickNum = e.ClickNum };
}
