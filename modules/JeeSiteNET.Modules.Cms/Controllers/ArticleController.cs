using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Cms.Application.DTOs;
using JeeSiteNET.Modules.Cms.Application.Services;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Cms.Controllers;

/// <summary>CMS 文章管理接口控制器，提供文章分页查询、详情、保存、删除、点击数记录等接口。</summary>
[ApiController]
[Route("api/v1/cms/article")]
public class ArticleController : ControllerBase

{
    private readonly ArticleService _articleService;

    public ArticleController(ArticleService articleService) => _articleService = articleService;

    /// <summary>HTTP POST - 分页查询列表，支持筛选与排序条件。</summary>
    [AllowAnonymous]
    [HttpPost("list")]
    public async Task<ApiResult<PageResult<ArticleDto>>> List([FromBody] PageRequest<Article> request)

        => ApiResult<PageResult<ArticleDto>>.Ok(await _articleService.FindPageAsync(request));

    /// <summary>HTTP GET - 根据主键获取单条详情。</summary>
    [AllowAnonymous]
    [HttpGet("get")]
    public async Task<ApiResult<ArticleDto?>> Get([FromQuery] string articleCode)
    {
        var dto = await _articleService.GetAsync(articleCode);

        return dto == null ? ApiResult<ArticleDto?>.NotFound("文章不存在") : ApiResult<ArticleDto?>.Ok(dto);
    }

    /// <summary>HTTP POST - 新增或更新实体信息。</summary>
    [Permission("cms:article:edit")]
    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] ArticleSaveDto dto) => await _articleService.SaveAsync(dto);

    /// <summary>HTTP POST - 删除指定实体（软删或硬删）。</summary>
    [Permission("cms:article:delete")]
    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteArticleRequest request) => await _articleService.DeleteAsync(request.ArticleCode);

    /// <summary>HTTP POST - 记录一次文章点击，自增点击数并清除缓存。</summary>
    [AllowAnonymous]
    [HttpPost("click")]
    public async Task<ApiResult> Click([FromQuery] string articleCode) => await _articleService.RecordClickAsync(articleCode);
}

/// <summary>文章删除请求 DTO。</summary>

public class DeleteArticleRequest { public string ArticleCode { get; set; } = string.Empty; }
