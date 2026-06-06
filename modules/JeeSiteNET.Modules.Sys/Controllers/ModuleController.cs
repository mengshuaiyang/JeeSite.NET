using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/module")]
public class ModuleController : ControllerBase
{
    private readonly ModuleService _moduleService;
    public ModuleController(ModuleService moduleService) => _moduleService = moduleService;

    [Permission("sys:module:list")]
    [HttpPost("list")]
    public async Task<ApiResult<PageResult<ModuleDto>>> List([FromBody] PageRequest<Module> request)
    {
        return ApiResult<PageResult<ModuleDto>>.Ok(await _moduleService.FindPageAsync(request));
    }

    [Permission("sys:module:list")]
    [HttpGet("get")]
    public async Task<ApiResult<ModuleDto?>> Get([FromQuery] string moduleCode)
    {
        var entity = await _moduleService.GetAsync(moduleCode);
        return entity == null ? ApiResult<ModuleDto?>.NotFound("模块不存在") : ApiResult<ModuleDto?>.Ok(entity);
    }

    [Permission("sys:module:edit")]
    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] ModuleSaveDto dto) => await _moduleService.SaveAsync(dto);

    [Permission("sys:module:delete")]
    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteModuleRequest request) => await _moduleService.DeleteAsync(request.ModuleCode);
}

public class DeleteModuleRequest { public string ModuleCode { get; set; } = string.Empty; }
