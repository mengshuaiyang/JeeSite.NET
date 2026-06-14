using JeeSiteNET.Core;
using JeeSiteNET.Modules.Cms.Application.DTOs;
using JeeSiteNET.Modules.Cms.Application.Services;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Cms.Controllers;

/// <summary>CMS 前台接口控制器，提供站点信息、栏目列表、文章列表/详情/搜索、标签云等面向访客的接口。</summary>
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

    /// <summary>HTTP GET - 获取所有站点信息列表。</summary>
    [HttpGet("site")]
    public async Task<ApiResult<List<SiteDto>>> GetSites()

        => ApiResult<List<SiteDto>>.Ok(await _siteService.GetAllAsync());

    /// <summary>HTTP GET - 按站点获取栏目列表。</summary>
    [AllowAnonymous]
    [HttpGet("category/list/{siteCode}")]
    public async Task<ApiResult<List<CategoryDto>>> GetCategories(string siteCode)

        => ApiResult<List<CategoryDto>>.Ok(await _categoryService.GetBySiteAsync(siteCode));

    /// <summary>HTTP POST - 前台文章分页列表接口。</summary>
    [HttpPost("article/list")]
    public async Task<ApiResult<PageResult<ArticleDto>>> ArticleList([FromBody] PageRequest<Article> request)

        => ApiResult<PageResult<ArticleDto>>.Ok(await _articleService.FindPageAsync(request));

    /// <summary>HTTP GET - 前台按文章编码获取文章详情。</summary>
    [HttpGet("article/get/{articleCode}")]
    public async Task<ApiResult<ArticleDto?>> ArticleGet(string articleCode)
    {
        var dto = await _articleService.GetAsync(articleCode);

        return dto == null ? ApiResult<ArticleDto?>.NotFound() : ApiResult<ArticleDto?>.Ok(dto);
    }

    /// <summary>HTTP POST - 前台文章搜索（关键字 + 分页）。</summary>
    [HttpPost("article/search")]
    public async Task<ApiResult<PageResult<ArticleDto>>> ArticleSearch([FromBody] PageRequest<Article> request)

        => ApiResult<PageResult<ArticleDto>>.Ok(await _articleService.FindPageAsync(request));

    /// <summary>HTTP GET - 获取文章标签云聚合结果。</summary>
    [HttpGet("tag/cloud")]
    public async Task<ApiResult<List<TagDto>>> TagCloud()

        => ApiResult<List<TagDto>>.Ok(await _articleService.GetTagCloudAsync());
}
