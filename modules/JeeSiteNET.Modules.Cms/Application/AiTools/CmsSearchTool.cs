    // 引入 JeeSiteNET.Core.AiTools 命名空间
// 引入命名空间：JeeSiteNET.Core.AiTools
using JeeSiteNET.Core.AiTools;
    // 引入 JeeSiteNET.Modules.Cms.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Interfaces
using JeeSiteNET.Modules.Cms.Domain.Interfaces;

// 定义 JeeSiteNET.Modules.Cms.Application.AiTools 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Application.AiTools
namespace JeeSiteNET.Modules.Cms.Application.AiTools;

[AiTool("cms_search", "搜索 CMS 文章内容", "CMS")]
// 定义class CmsSearchTool
// 定义类：CmsSearchTool
public class CmsSearchTool : IAiTool
{
    // 字段 _articleRepository
    // 字段：_articleRepository
    private readonly IArticleRepository _articleRepository;

    public string Name => "cms_search";
    public string Description => "搜索 CMS 文章内容";

    // 方法 CmsSearchTool
    // 构造函数：CmsSearchTool
    public CmsSearchTool(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    // 方法 ExecuteAsync
    // 方法：ExecuteAsync
    public async Task<AiToolResult> ExecuteAsync(AiToolContext context, CancellationToken cancellationToken = default)
    {
        // 声明并初始化变量：keyword
        var keyword = context.GetParameter("keyword") ?? context.UserMessage;
        // if 条件判断
        if (string.IsNullOrWhiteSpace(keyword))
            // return 返回结果
            return AiToolResult.Fail("请输入搜索关键词");

        var all = await _articleRepository.FindListAsync();
        // 声明并初始化变量：matched
        var matched = all
            // 集合操作：检查是否包含
            .Where(a => (a.Title?.Contains(keyword) ?? false) || (a.Description?.Contains(keyword) ?? false))
            .Take(5)
            // 数据库操作：查询为列表
            .ToList();

        // if 条件判断
        if (matched.Count == 0)
            // return 返回结果
            return AiToolResult.Ok("未找到相关文章");

        // 数据库操作：投影选择
        var results = string.Join("\n\n", matched.Select(a =>
            $"- [{a.Title}]\n  {a.Description}"));

        // return 返回结果
        return AiToolResult.Ok($"找到 {matched.Count} 篇相关文章:\n\n{results}");
    }
}
