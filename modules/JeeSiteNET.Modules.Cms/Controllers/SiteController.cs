using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Cms.Application.DTOs;
using JeeSiteNET.Modules.Cms.Application.Services;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Cms.Controllers;

[ApiController]
[Route("api/v1/cms/site")]
public class SiteController : ControllerBase
{
    private readonly SiteService _siteService;
    public SiteController(SiteService siteService) => _siteService = siteService;

    [AllowAnonymous]
    [HttpPost("list")]
    public async Task<ApiResult<PageResult<SiteDto>>> List([FromBody] PageRequest<Site> request)
        => ApiResult<PageResult<SiteDto>>.Ok(await _siteService.FindPageAsync(request));

    [AllowAnonymous]
    [HttpGet("get")]
    public async Task<ApiResult<SiteDto?>> Get([FromQuery] string siteCode)
    {
        var dto = await _siteService.GetAsync(siteCode);
        return dto == null ? ApiResult<SiteDto?>.NotFound("站点不存在") : ApiResult<SiteDto?>.Ok(dto);
    }

    [Permission("cms:site:edit")]
    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] SiteSaveDto dto) => await _siteService.SaveAsync(dto);

    [Permission("cms:site:delete")]
    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteSiteRequest request) => await _siteService.DeleteAsync(request.SiteCode);
}

public class DeleteSiteRequest { public string SiteCode { get; set; } = string.Empty; }
