using JeeSiteNET.Core;

using JeeSiteNET.Core.Utils;

using JeeSiteNET.Modules.Cms.Application.DTOs;

using JeeSiteNET.Modules.Cms.Domain.Entities;

using JeeSiteNET.Modules.Cms.Domain.Interfaces;

using Microsoft.EntityFrameworkCore;

using ZiggyCreatures.Caching.Fusion;

namespace JeeSiteNET.Modules.Cms.Application.Services;
/// <summary>CMS 文章领域服务，提供文章分页查询、详情缓存、保存、删除、点击数统计以及标签云聚合。</summary>
public class ArticleService

{

    private readonly IArticleRepository _articleRepository;

    private readonly ICategoryRepository _categoryRepository;

    private readonly IArticleTagRepository _articleTagRepository;

    private readonly IFusionCache _cache;

    public ArticleService(IArticleRepository articleRepository, ICategoryRepository categoryRepository, IArticleTagRepository articleTagRepository, IFusionCache cache)

    {

        _articleRepository = articleRepository;

        _categoryRepository = categoryRepository;

        _articleTagRepository = articleTagRepository;

        _cache = cache;

    }
    /// <summary>获取单条数据的异步方法（详情查询）。</summary>
    public async Task<ArticleDto?> GetAsync(string articleCode)

    {

        return await _cache.GetOrSetAsync(

            CacheKeys.CmsArticle(articleCode),

            async ct =>

            {

                var entity = await _articleRepository.GetWithDetailAsync(articleCode);

                if (entity == null) return null;

                var category = await _categoryRepository.GetAsync(entity.CategoryCode);

                return ArticleDto.FromEntity(entity, category?.CategoryName);

            },

            TimeSpan.FromMinutes(10));

    }
    /// <summary>分页查询方法（按条件返回分页结果）。</summary>
    public async Task<PageResult<ArticleDto>> FindPageAsync(PageRequest<Article> request)

    {

        var result = await _articleRepository.FindPageAsync(request);

        var catCodes = result.List.Select(e => e.CategoryCode).Where(c => !string.IsNullOrEmpty(c)).Distinct().ToList();

        var cats = catCodes.Count > 0

            ? await _categoryRepository.FindByCodesAsync(catCodes)

            : [];

        var catMap = cats.ToDictionary(c => c.CategoryCode, c => c.CategoryName);

        var dtos = result.List.Select(e => ArticleDto.FromEntity(e, catMap.GetValueOrDefault(e.CategoryCode))).ToList();

        return new PageResult<ArticleDto> { List = dtos, Total = result.Total, PageNo = result.PageNo, PageSize = result.PageSize };

    }
    /// <summary>保存方法（新增或更新）。</summary>
    public async Task<ApiResult> SaveAsync(ArticleSaveDto dto)

    {

        var now = DateTime.Now;

        var safeContent = string.IsNullOrEmpty(dto.Content) ? null : HtmlSanitizerUtil.SanitizeRich(dto.Content);

        var safeTitle = string.IsNullOrEmpty(dto.Title) ? null : HtmlSanitizerUtil.SanitizeStrict(dto.Title);

        var safeSubtitle = string.IsNullOrEmpty(dto.Subtitle) ? null : HtmlSanitizerUtil.SanitizeStrict(dto.Subtitle);

        var safeSummary = string.IsNullOrEmpty(dto.Summary) ? null : HtmlSanitizerUtil.SanitizeStrict(dto.Summary);

        var safeAuthor = string.IsNullOrEmpty(dto.Author) ? null : HtmlSanitizerUtil.SanitizeStrict(dto.Author);

        var safeSource = string.IsNullOrEmpty(dto.Source) ? null : HtmlSanitizerUtil.SanitizeStrict(dto.Source);

        var safeImage = string.IsNullOrEmpty(dto.Image) ? null : HtmlSanitizerUtil.SanitizeStrict(dto.Image);

        var safeTags = string.IsNullOrEmpty(dto.Tags) ? null : HtmlSanitizerUtil.SanitizeStrict(dto.Tags);

        Article? entity;

        if (!string.IsNullOrEmpty(dto.ArticleCode))

        {

            entity = await _articleRepository.GetWithDetailAsync(dto.ArticleCode);

            if (entity == null) return ApiResult.NotFound("文章不存在");

            entity.CategoryCode = dto.CategoryCode ?? string.Empty; entity.Title = safeTitle ?? string.Empty; entity.Subtitle = safeSubtitle ?? string.Empty;

            entity.Summary = safeSummary; entity.Author = safeAuthor;

            entity.Source = safeSource; entity.Image = safeImage; entity.Tags = safeTags;

            entity.IsTop = dto.IsTop; entity.IsRecommend = dto.IsRecommend; entity.IsHot = dto.IsHot;

            entity.PublishDate = dto.PublishDate; entity.UpdateDate = now;

            if (entity.ArticleData != null)

                entity.ArticleData.Content = safeContent;

            else if (!string.IsNullOrEmpty(safeContent))

                entity.ArticleData = new ArticleData { ArticleCode = entity.ArticleCode, Content = safeContent };

            await _articleRepository.UpdateAsync(entity);

        }

        else

        {

            entity = new Article

            {

                ArticleCode = Guid.NewGuid().ToString("N")[..20],

                CategoryCode = dto.CategoryCode ?? string.Empty, Title = safeTitle ?? string.Empty, Subtitle = safeSubtitle ?? string.Empty,

                Summary = safeSummary, Author = safeAuthor,

                Source = safeSource, Image = safeImage, Tags = safeTags,

                IsTop = dto.IsTop ?? "0", IsRecommend = dto.IsRecommend ?? "0", IsHot = dto.IsHot ?? "0",

                ClickCount = 0, PublishDate = dto.PublishDate ?? now,

                CreateDate = now, UpdateDate = now

            };

            if (!string.IsNullOrEmpty(safeContent))

                entity.ArticleData = new ArticleData { ArticleCode = entity.ArticleCode, Content = safeContent };

            await _articleRepository.AddAsync(entity);

        }

        await _cache.RemoveAsync(CacheKeys.CmsArticle(entity.ArticleCode));

        return ApiResult.Ok(ArticleDto.FromEntity(entity));

    }
    /// <summary>删除方法。</summary>
    public async Task<ApiResult> DeleteAsync(string articleCode)

    {

        var entity = await _articleRepository.GetAsync(articleCode);

        if (entity == null) return ApiResult.NotFound("文章不存在");

        await _articleRepository.DeleteAsync(entity);

        await _cache.RemoveAsync(CacheKeys.CmsArticle(articleCode));

        return ApiResult.Ok();

    }
    /// <summary>记录一次文章点击并使缓存失效。</summary>
    public async Task<ApiResult> RecordClickAsync(string articleCode)

    {

        var entity = await _articleRepository.GetAsync(articleCode);

        if (entity == null) return ApiResult.NotFound("文章不存在");

        entity.ClickCount = (entity.ClickCount ?? 0) + 1;

        await _articleRepository.UpdateAsync(entity);

        await _cache.RemoveAsync(CacheKeys.CmsArticle(articleCode));

        return ApiResult.Ok();

    }
    /// <summary>聚合标签使用情况并返回标签云。</summary>
    public async Task<List<TagDto>> GetTagCloudAsync()

    {

        var tagCounts = await _articleTagRepository.Query()

            .GroupBy(at => at.TagName)

            .Select(g => new { TagName = g.Key, Count = g.Count() })

            .ToListAsync();

        return tagCounts.Select(tc => new TagDto { TagName = tc.TagName, ArticleCount = tc.Count }).ToList();

    }

}
