using JeeSiteNET.Core;
using JeeSiteNET.Modules.Cms.Application.DTOs;
using JeeSiteNET.Modules.Cms.Application.Services;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Cms.Controllers;

[ApiController]
[Route("api/v1/cms/front")]
[AllowAnonymous]
public class CmsFrontController : ControllerBase
{
    private readonly ArticleService _articleService;
    private readonly CategoryService _categoryService;
    private readonly SiteService _siteService;

    public CmsFrontController(ArticleService articleService, CategoryService categoryService, SiteService siteService)
    {
        _articleService = articleService;
        _categoryService = categoryService;
        _siteService = siteService;
    }

    [HttpGet("site")]
    public async Task<ApiResult<List<SiteDto>>> GetSites()
        => ApiResult<List<SiteDto>>.Ok(await _siteService.GetAllAsync());

    [AllowAnonymous]
    [HttpGet("category/list/{siteCode}")]
    public async Task<ApiResult<List<CategoryDto>>> GetCategories(string siteCode)
        => ApiResult<List<CategoryDto>>.Ok(await _categoryService.GetBySiteAsync(siteCode));

    [HttpPost("article/list")]
    public async Task<ApiResult<PageResult<ArticleDto>>> ArticleList([FromBody] PageRequest<Article> request)
        => ApiResult<PageResult<ArticleDto>>.Ok(await _articleService.FindPageAsync(request));

    [HttpGet("article/get/{articleCode}")]
    public async Task<ApiResult<ArticleDto?>> ArticleGet(string articleCode)
    {
        var dto = await _articleService.GetAsync(articleCode);
        return dto == null ? ApiResult<ArticleDto?>.NotFound() : ApiResult<ArticleDto?>.Ok(dto);
    }

    [HttpPost("article/search")]
    public async Task<ApiResult<PageResult<ArticleDto>>> ArticleSearch([FromBody] PageRequest<Article> request)
        => ApiResult<PageResult<ArticleDto>>.Ok(await _articleService.FindPageAsync(request));

    [HttpGet("tag/cloud")]
    public async Task<ApiResult<List<TagDto>>> TagCloud()
        => ApiResult<List<TagDto>>.Ok(await _articleService.GetTagCloudAsync());
}
