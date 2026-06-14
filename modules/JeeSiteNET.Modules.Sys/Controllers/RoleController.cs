using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

/// <summary>角色管理接口控制器，提供角色分页查询、详情获取、新增或编辑、删除等后台管理接口。</summary>
[ApiController]
[Route("api/v1/sys/role")]
public class RoleController : ControllerBase

{
    private readonly RoleService _roleService;

    public RoleController(RoleService roleService) => _roleService = roleService;

    /// <summary>HTTP POST - 分页查询列表，支持筛选与排序条件。</summary>
    [Permission("sys:role:list")]
    [HttpPost("list")]
    public async Task<ApiResult<PageResult<RoleDto>>> List([FromBody] PageRequest<Role> request)
    {
        var result = await _roleService.FindPageAsync(request);

        return ApiResult<PageResult<RoleDto>>.Ok(result);
    }

    /// <summary>HTTP GET - 根据主键获取单条详情。</summary>
    [Permission("sys:role:list")]
    [HttpGet("get")]
    public async Task<ApiResult<RoleDto?>> Get([FromQuery] string roleCode)
    {
        var role = await _roleService.GetAsync(roleCode);

        if (role == null) return ApiResult<RoleDto?>.NotFound("角色不存在");

        return ApiResult<RoleDto?>.Ok(role);
    }

    /// <summary>HTTP POST - 新增或更新实体信息。</summary>
    [Permission("sys:role:edit")]
    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] RoleSaveDto dto)
    {
        return await _roleService.SaveAsync(dto);
    }

    /// <summary>HTTP POST - 删除指定实体（软删或硬删）。</summary>
    [Permission("sys:role:delete")]
    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteRoleRequest request)
    {
        return await _roleService.DeleteAsync(request.RoleCode);
    }
}

/// <summary>角色删除请求 DTO。</summary>

public class DeleteRoleRequest

{
    public string RoleCode { get; set; } = string.Empty;
}
