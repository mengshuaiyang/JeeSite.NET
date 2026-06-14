    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services
using JeeSiteNET.Modules.Sys.Application.Services;
    // 引入 Microsoft.AspNetCore.Authorization 命名空间
// 引入命名空间：Microsoft.AspNetCore.Authorization
using Microsoft.AspNetCore.Authorization;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/lang")]
// 定义class LangController
// 定义类：LangController

public class LangController : ControllerBase
{
    // 字段 _langService
    // 字段：_langService

    private readonly LangService _langService;
    // 构造函数 LangController
    // 构造函数：LangController

    public LangController(LangService langService) => _langService = langService;

    [Permission("sys:lang:list")]
    [HttpGet("list")]
    // 方法：List

    public async Task<ApiResult<List<LangDto>>> List()
        => ApiResult<List<LangDto>>.Ok(await _langService.GetAllAsync());

    [AllowAnonymous]
    [HttpGet("get-by-type")]
    // 方法 GetByType
    // 方法：GetByType

    public async Task<ApiResult<List<LangDto>>> GetByType([FromQuery] string langType)
        => ApiResult<List<LangDto>>.Ok(await _langService.GetByLangTypeAsync(langType));

    [Permission("sys:lang:edit")]
    [HttpPost("save")]
    // 方法：Save

    public async Task<ApiResult> Save([FromBody] LangSaveDto dto) => await _langService.SaveAsync(dto);

    [Permission("sys:lang:delete")]
    [HttpPost("delete")]
    // 方法：Delete

    public async Task<ApiResult> Delete([FromBody] DeleteLangRequest request) => await _langService.DeleteAsync(request.Id);
}

// 定义class DeleteLangRequest
// 定义类：DeleteLangRequest

public class DeleteLangRequest { public string Id { get; set; } = string.Empty; }
