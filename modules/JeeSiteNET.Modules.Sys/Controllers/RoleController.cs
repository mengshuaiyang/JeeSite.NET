using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/role")]
public class RoleController : ControllerBase
{
    private readonly RoleService _roleService;

    public RoleController(RoleService roleService) => _roleService = roleService;

    [HttpPost("list")]
    public async Task<ApiResult<PageResult<RoleDto>>> List([FromBody] PageRequest<Role> request)
    {
        var result = await _roleService.FindPageAsync(request);
        return ApiResult<PageResult<RoleDto>>.Ok(result);
    }

    [HttpGet("get")]
    public async Task<ApiResult<RoleDto?>> Get([FromQuery] string roleCode)
    {
        var role = await _roleService.GetAsync(roleCode);
        if (role == null) return ApiResult<RoleDto?>.NotFound("角色不存在");
        return ApiResult<RoleDto?>.Ok(role);
    }

    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] RoleSaveDto dto)
    {
        return await _roleService.SaveAsync(dto);
    }

    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteRoleRequest request)
    {
        return await _roleService.DeleteAsync(request.RoleCode);
    }
}

public class DeleteRoleRequest
{
    public string RoleCode { get; set; } = string.Empty;
}
