using JeeSiteNET.Modules.Cms.Domain.Entities;
using ZiggyCreatures.Caching.Fusion;

namespace JeeSiteNET.Modules.Cms.Application.Services;

// ================================================================
// CMS 页面缓存服务
//
// 作用：缓存站点首页、栏目列表、文章详情等 CMS 前端页面数据，
//       减少数据库查询频率，提升访问速度。
//
// 底层缓存是 FusionCache（L1 内存 + L2 Redis），
// 缓存键统一前缀 "CmsPage:"，便于统一管理和清理。
//
// 缓存失效策略：
//   更新/删除文章 → 清除该文章+所属栏目缓存（ClearArticleCacheAsync）
//   更新/删除栏目 → 清除该栏目+站点下所有栏目列表缓存（ClearCategoryCacheAsync）
//   更新/删除站点 → 清除该站点+栏目列表缓存（ClearSiteCacheAsync）
//
// 在 CmsModuleInstaller.cs 中注册为 Scoped 服务。
// 调用方通常是 CmsService / ArticleService / CategoryService 等。
// ================================================================

public class PageCacheService
{
    private readonly IFusionCache _cache;
    private static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromMinutes(30);
    private const string KeyPrefix = "CmsPage:";

    public PageCacheService(IFusionCache cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// 从缓存获取数据，不存在则用 factory 函数加载并回填缓存。
    /// 相当于"有就读缓存，没有就查数据库"的通用模式。
    /// </summary>
    /// <typeparam name="T">缓存值类型</typeparam>
    /// <param name="key">缓存键（自动拼接 "CmsPage:" 前缀）</param>
    /// <param name="factory">缓存未命中时执行的回调（从数据库加载数据）</param>
    /// <param name="duration">缓存时长，默认 30 分钟</param>
    public async Task<T?> GetOrSetAsync<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan? duration = null)
    {
        return await _cache.GetOrSetAsync($"{KeyPrefix}{key}", factory, duration ?? DefaultCacheDuration);
    }

    /// <summary>文章变更时清除相关缓存：文章详情 + 所属栏目列表</summary>
    public async Task ClearArticleCacheAsync(Article article)
    {
        if (article == null) return;
        await _cache.RemoveAsync($"{KeyPrefix}Article:{article.ArticleCode}");
        await _cache.RemoveAsync($"{KeyPrefix}Category:{article.CategoryCode}");
        await _cache.RemoveAsync($"{KeyPrefix}CategoryList:{article.CategoryCode}");
    }

    /// <summary>栏目变更时清除相关缓存：栏目详情 + 所属站点下栏目列表</summary>
    public async Task ClearCategoryCacheAsync(Category category)
    {
        if (category == null) return;
        await _cache.RemoveAsync($"{KeyPrefix}Category:{category.CategoryCode}");
        await _cache.RemoveAsync($"{KeyPrefix}CategoryList:{category.SiteCode}");
    }

    /// <summary>站点变更时清除相关缓存：站点详情 + 栏目列表</summary>
    public async Task ClearSiteCacheAsync(Site site)
    {
        if (site == null) return;
        await _cache.RemoveAsync($"{KeyPrefix}Site:{site.SiteCode}");
        await _cache.RemoveAsync($"{KeyPrefix}CategoryList:{site.SiteCode}");
    }

    /// <summary>清空所有 CMS 页面缓存（站点和栏目）</summary>
    public async Task ClearAllAsync()
    {
        await _cache.RemoveAsync($"{KeyPrefix}Site:all");
        await _cache.RemoveAsync($"{KeyPrefix}Category:all");
    }
}
