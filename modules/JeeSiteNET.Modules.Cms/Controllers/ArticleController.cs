using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Cms.Application.DTOs;
using JeeSiteNET.Modules.Cms.Application.Services;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Cms.Controllers;

[ApiController]
[Route("api/v1/cms/article")]
public class ArticleController : ControllerBase
{
    private readonly ArticleService _articleService;
    public ArticleController(ArticleService articleService) => _articleService = articleService;

    [AllowAnonymous]
    [HttpPost("list")]
    public async Task<ApiResult<PageResult<ArticleDto>>> List([FromBody] PageRequest<Article> request)
        => ApiResult<PageResult<ArticleDto>>.Ok(await _articleService.FindPageAsync(request));

    [AllowAnonymous]
    [HttpGet("get")]
    public async Task<ApiResult<ArticleDto?>> Get([FromQuery] string articleCode)
    {
        var dto = await _articleService.GetAsync(articleCode);
        return dto == null ? ApiResult<ArticleDto?>.NotFound("文章不存在") : ApiResult<ArticleDto?>.Ok(dto);
    }

    [Permission("cms:article:edit")]
    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] ArticleSaveDto dto) => await _articleService.SaveAsync(dto);

    [Permission("cms:article:delete")]
    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteArticleRequest request) => await _articleService.DeleteAsync(request.ArticleCode);

    [AllowAnonymous]
    [HttpPost("click")]
    public async Task<ApiResult> Click([FromQuery] string articleCode) => await _articleService.RecordClickAsync(articleCode);
}

public class DeleteArticleRequest { public string ArticleCode { get; set; } = string.Empty; }
