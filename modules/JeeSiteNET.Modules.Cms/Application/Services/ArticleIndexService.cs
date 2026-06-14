    // 引入 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
using JeeSiteNET.Modules.Cms.Domain.Entities;

// 定义 JeeSiteNET.Modules.Cms.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Application.Services
namespace JeeSiteNET.Modules.Cms.Application.Services;

// 定义接口 IArticleIndexService
// 定义接口：IArticleIndexService
public interface IArticleIndexService
{
    Task IndexAsync(Article article);
    // 缓存：移除项
    Task RemoveAsync(string articleCode);
}

// 定义class DefaultArticleIndexService
// 定义类：DefaultArticleIndexService
public class DefaultArticleIndexService : IArticleIndexService
{
    // 方法：IndexAsync
    public Task IndexAsync(Article article) => Task.CompletedTask;
    // 方法：RemoveAsync
    public Task RemoveAsync(string articleCode) => Task.CompletedTask;
}
