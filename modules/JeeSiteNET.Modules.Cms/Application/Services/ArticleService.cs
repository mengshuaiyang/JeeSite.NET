using JeeSiteNET.Core;
using JeeSiteNET.Modules.Cms.Application.DTOs;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Cms.Application.Services;

public class ArticleService
{
    private readonly IArticleRepository _articleRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IArticleTagRepository _articleTagRepository;

    public ArticleService(IArticleRepository articleRepository, ICategoryRepository categoryRepository, IArticleTagRepository articleTagRepository)
    {
        _articleRepository = articleRepository;
        _categoryRepository = categoryRepository;
        _articleTagRepository = articleTagRepository;
    }

    public async Task<ArticleDto?> GetAsync(string articleCode)
    {
        var entity = await _articleRepository.GetWithDetailAsync(articleCode);
        if (entity == null) return null;
        var category = await _categoryRepository.GetAsync(entity.CategoryCode);
        return ArticleDto.FromEntity(entity, category?.CategoryName);
    }

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

    public async Task<ApiResult> SaveAsync(ArticleSaveDto dto)
    {
        var now = DateTime.Now;
        Article? entity;
        if (!string.IsNullOrEmpty(dto.ArticleCode))
        {
            entity = await _articleRepository.GetWithDetailAsync(dto.ArticleCode);
            if (entity == null) return ApiResult.NotFound("文章不存在");
            entity.CategoryCode = dto.CategoryCode; entity.Title = dto.Title; entity.Subtitle = dto.Subtitle;
            entity.Summary = dto.Summary; entity.Author = dto.Author;
            entity.Source = dto.Source; entity.Image = dto.Image; entity.Tags = dto.Tags;
            entity.IsTop = dto.IsTop; entity.IsRecommend = dto.IsRecommend; entity.IsHot = dto.IsHot;
            entity.PublishDate = dto.PublishDate; entity.UpdateDate = now;
            if (entity.ArticleData != null)
                entity.ArticleData.Content = dto.Content;
            else if (!string.IsNullOrEmpty(dto.Content))
                entity.ArticleData = new ArticleData { ArticleCode = entity.ArticleCode, Content = dto.Content };
            await _articleRepository.UpdateAsync(entity);
        }
        else
        {
            entity = new Article
            {
                ArticleCode = Guid.NewGuid().ToString("N")[..20],
                CategoryCode = dto.CategoryCode, Title = dto.Title, Subtitle = dto.Subtitle,
                Summary = dto.Summary, Author = dto.Author,
                Source = dto.Source, Image = dto.Image, Tags = dto.Tags,
                IsTop = dto.IsTop ?? "0", IsRecommend = dto.IsRecommend ?? "0", IsHot = dto.IsHot ?? "0",
                ClickCount = 0, PublishDate = dto.PublishDate ?? now,
                CreateDate = now, UpdateDate = now
            };
            if (!string.IsNullOrEmpty(dto.Content))
                entity.ArticleData = new ArticleData { ArticleCode = entity.ArticleCode, Content = dto.Content };
            await _articleRepository.AddAsync(entity);
        }
        return ApiResult.Ok(ArticleDto.FromEntity(entity));
    }

    public async Task<ApiResult> DeleteAsync(string articleCode)
    {
        var entity = await _articleRepository.GetAsync(articleCode);
        if (entity == null) return ApiResult.NotFound("文章不存在");
        await _articleRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    public async Task<ApiResult> RecordClickAsync(string articleCode)
    {
        var entity = await _articleRepository.GetAsync(articleCode);
        if (entity == null) return ApiResult.NotFound("文章不存在");
        entity.ClickCount = (entity.ClickCount ?? 0) + 1;
        await _articleRepository.UpdateAsync(entity);
        return ApiResult.Ok();
    }

    public async Task<List<TagDto>> GetTagCloudAsync()
    {
        var tagCounts = await _articleTagRepository.Query()
            .GroupBy(at => at.TagName)
            .Select(g => new { TagName = g.Key, Count = g.Count() })
            .ToListAsync();
        return tagCounts.Select(tc => new TagDto { TagName = tc.TagName, ArticleCount = tc.Count }).ToList();
    }
}
