using JeeSiteNET.Core.AiTools;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;

namespace JeeSiteNET.Modules.Cms.Application.AiTools;

[AiTool("cms_search", "搜索 CMS 文章内容", "CMS")]
public class CmsSearchTool : IAiTool
{
    private readonly IArticleRepository _articleRepository;

    public string Name => "cms_search";
    public string Description => "搜索 CMS 文章内容";

    public CmsSearchTool(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public async Task<AiToolResult> ExecuteAsync(AiToolContext context, CancellationToken cancellationToken = default)
    {
        var keyword = context.GetParameter("keyword") ?? context.UserMessage;
        if (string.IsNullOrWhiteSpace(keyword))
            return AiToolResult.Fail("请输入搜索关键词");

        var all = await _articleRepository.FindListAsync();
        var matched = all
            .Where(a => (a.Title?.Contains(keyword) ?? false) || (a.Description?.Contains(keyword) ?? false))
            .Take(5)
            .ToList();

        if (matched.Count == 0)
            return AiToolResult.Ok("未找到相关文章");

        var results = string.Join("\n\n", matched.Select(a =>
            $"- [{a.Title}]\n  {a.Description}"));

        return AiToolResult.Ok($"找到 {matched.Count} 篇相关文章:\n\n{results}");
    }
}
