    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services
using JeeSiteNET.Modules.Sys.Application.Services;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
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
[Route("api/v1/sys/module")]
// 定义class ModuleController
// 定义类：ModuleController

public class ModuleController : ControllerBase
{
    // 字段 _moduleService
    // 字段：_moduleService

    private readonly ModuleService _moduleService;
    // 构造函数 ModuleController
    // 构造函数：ModuleController

    public ModuleController(ModuleService moduleService) => _moduleService = moduleService;

    [Permission("sys:module:list")]
    [HttpPost("list")]
    // 方法：List

    public async Task<ApiResult<PageResult<ModuleDto>>> List([FromBody] PageRequest<Module> request)
    {
        // return 返回结果
        return ApiResult<PageResult<ModuleDto>>.Ok(await _moduleService.FindPageAsync(request));
    }

    [Permission("sys:module:list")]
    [HttpGet("get")]
    // 方法 Get
    // 方法：Get

    public async Task<ApiResult<ModuleDto?>> Get([FromQuery] string moduleCode)
    {
        // 缓存：获取值
        var entity = await _moduleService.GetAsync(moduleCode);
        // return 返回结果
        return entity == null ? ApiResult<ModuleDto?>.NotFound("模块不存在") : ApiResult<ModuleDto?>.Ok(entity);
    }

    [Permission("sys:module:edit")]
    [HttpPost("save")]
    // 方法：Save

    public async Task<ApiResult> Save([FromBody] ModuleSaveDto dto) => await _moduleService.SaveAsync(dto);

    [Permission("sys:module:delete")]
    [HttpPost("delete")]
    // 方法：Delete

    public async Task<ApiResult> Delete([FromBody] DeleteModuleRequest request) => await _moduleService.DeleteAsync(request.ModuleCode);
}

// 定义class DeleteModuleRequest
// 定义类：DeleteModuleRequest

public class DeleteModuleRequest { public string ModuleCode { get; set; } = string.Empty; }
