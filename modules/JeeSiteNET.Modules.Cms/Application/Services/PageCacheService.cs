    // 引入 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
using JeeSiteNET.Modules.Cms.Domain.Entities;
    // 引入 ZiggyCreatures.Caching.Fusion 命名空间
// 引入命名空间：ZiggyCreatures.Caching.Fusion
using ZiggyCreatures.Caching.Fusion;

// 定义 JeeSiteNET.Modules.Cms.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Application.Services
namespace JeeSiteNET.Modules.Cms.Application.Services;

// 定义class PageCacheService
// 定义类：PageCacheService
public class PageCacheService
{
    // 字段 _cache
    // 字段：_cache
    private readonly IFusionCache _cache;

    private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(30);

    private const string KeyPrefix = "CmsPage:";

    // 方法 PageCacheService
    // 构造函数：PageCacheService
    public PageCacheService(IFusionCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetOrSetAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan? duration = null)
    {
        // return 返回结果
        return await _cache.GetOrSetAsync($"{KeyPrefix}{key}", factory, duration ?? DefaultCacheDuration);
    }

    // 方法 ClearArticleCacheAsync
    // 方法：ClearArticleCacheAsync
    public async Task ClearArticleCacheAsync(Article article)
    {
        // if 条件判断
        if (article == null) return;
        // await 异步等待
        await _cache.RemoveAsync($"{KeyPrefix}Article:{article.ArticleCode}");
        // await 异步等待
        await _cache.RemoveAsync($"{KeyPrefix}Category:{article.CategoryCode}");
        // await 异步等待
        await _cache.RemoveAsync($"{KeyPrefix}CategoryList:{article.CategoryCode}");
    }

    // 方法 ClearCategoryCacheAsync
    // 方法：ClearCategoryCacheAsync
    public async Task ClearCategoryCacheAsync(Category category)
    {
        // if 条件判断
        if (category == null) return;
        // await 异步等待
        await _cache.RemoveAsync($"{KeyPrefix}Category:{category.CategoryCode}");
        // await 异步等待
        await _cache.RemoveAsync($"{KeyPrefix}CategoryList:{category.SiteCode}");
    }

    // 方法 ClearSiteCacheAsync
    // 方法：ClearSiteCacheAsync
    public async Task ClearSiteCacheAsync(Site site)
    {
        // if 条件判断
        if (site == null) return;
        // await 异步等待
        await _cache.RemoveAsync($"{KeyPrefix}Site:{site.SiteCode}");
        // await 异步等待
        await _cache.RemoveAsync($"{KeyPrefix}CategoryList:{site.SiteCode}");
    }

    // 方法 ClearAllAsync
    // 方法：ClearAllAsync
    public async Task ClearAllAsync()
    {
        // await 异步等待
        await _cache.RemoveAsync($"{KeyPrefix}Site:all");
        // await 异步等待
        await _cache.RemoveAsync($"{KeyPrefix}Category:all");
    }
}
