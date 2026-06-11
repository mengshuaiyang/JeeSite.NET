using JeeSiteNET.Modules.Cms.Domain.Entities;
using ZiggyCreatures.Caching.Fusion;

namespace JeeSiteNET.Modules.Cms.Application.Services;

public class PageCacheService
{
    private readonly IFusionCache _cache;

    private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(30);

    private const string KeyPrefix = "CmsPage:";

    public PageCacheService(IFusionCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetOrSetAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan? duration = null)
    {
        return await _cache.GetOrSetAsync($"{KeyPrefix}{key}", factory, duration ?? DefaultCacheDuration);
    }

    public async Task ClearArticleCacheAsync(Article article)
    {
        if (article == null) return;
        await _cache.RemoveAsync($"{KeyPrefix}Article:{article.ArticleCode}");
        await _cache.RemoveAsync($"{KeyPrefix}Category:{article.CategoryCode}");
        await _cache.RemoveAsync($"{KeyPrefix}CategoryList:{article.CategoryCode}");
    }

    public async Task ClearCategoryCacheAsync(Category category)
    {
        if (category == null) return;
        await _cache.RemoveAsync($"{KeyPrefix}Category:{category.CategoryCode}");
        await _cache.RemoveAsync($"{KeyPrefix}CategoryList:{category.SiteCode}");
    }

    public async Task ClearSiteCacheAsync(Site site)
    {
        if (site == null) return;
        await _cache.RemoveAsync($"{KeyPrefix}Site:{site.SiteCode}");
        await _cache.RemoveAsync($"{KeyPrefix}CategoryList:{site.SiteCode}");
    }

    public async Task ClearAllAsync()
    {
        await _cache.RemoveAsync($"{KeyPrefix}Site:all");
        await _cache.RemoveAsync($"{KeyPrefix}Category:all");
    }
}
