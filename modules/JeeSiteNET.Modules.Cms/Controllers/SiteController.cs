    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Modules.Cms.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Application.DTOs
using JeeSiteNET.Modules.Cms.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Cms.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Application.Services
using JeeSiteNET.Modules.Cms.Application.Services;
    // 引入 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
using JeeSiteNET.Modules.Cms.Domain.Entities;
    // 引入 Microsoft.AspNetCore.Authorization 命名空间
// 引入命名空间：Microsoft.AspNetCore.Authorization
using Microsoft.AspNetCore.Authorization;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Cms.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Controllers
namespace JeeSiteNET.Modules.Cms.Controllers;

[ApiController]
[Route("api/v1/cms/site")]
// 定义class SiteController
// 定义类：SiteController

public class SiteController : ControllerBase
{
    // 字段 _siteService
    // 字段：_siteService

    private readonly SiteService _siteService;
    // 构造函数 SiteController
    // 构造函数：SiteController

    public SiteController(SiteService siteService) => _siteService = siteService;

    [AllowAnonymous]
    [HttpPost("list")]
    // 方法：List

    public async Task<ApiResult<PageResult<SiteDto>>> List([FromBody] PageRequest<Site> request)
        => ApiResult<PageResult<SiteDto>>.Ok(await _siteService.FindPageAsync(request));

    [AllowAnonymous]
    [HttpGet("get")]
    // 方法 Get
    // 方法：Get

    public async Task<ApiResult<SiteDto?>> Get([FromQuery] string siteCode)
    {
        // 缓存：获取值
        var dto = await _siteService.GetAsync(siteCode);
        // return 返回结果
        return dto == null ? ApiResult<SiteDto?>.NotFound("站点不存在") : ApiResult<SiteDto?>.Ok(dto);
    }

    [Permission("cms:site:edit")]
    [HttpPost("save")]
    // 方法：Save

    public async Task<ApiResult> Save([FromBody] SiteSaveDto dto) => await _siteService.SaveAsync(dto);

    [Permission("cms:site:delete")]
    [HttpPost("delete")]
    // 方法：Delete

    public async Task<ApiResult> Delete([FromBody] DeleteSiteRequest request) => await _siteService.DeleteAsync(request.SiteCode);
}

// 定义class DeleteSiteRequest
// 定义类：DeleteSiteRequest

public class DeleteSiteRequest { public string SiteCode { get; set; } = string.Empty; }
