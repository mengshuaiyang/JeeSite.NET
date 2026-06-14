using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

/// <summary>用户管理接口控制器，提供用户分页查询、详情获取、新增或编辑、删除等后台管理接口。</summary>
[ApiController]
[Route("api/v1/sys/user")]
public class UserController : ControllerBase

{
    private readonly UserService _userService;

    public UserController(UserService userService) => _userService = userService;

    /// <summary>HTTP POST - 分页查询列表，支持筛选与排序条件。</summary>
    [Permission("sys:user:list")]
    [HttpPost("list")]
    public async Task<ApiResult<PageResult<UserDto>>> List([FromBody] PageRequest<User> request)
    {
        var result = await _userService.FindPageAsync(request);

        return ApiResult<PageResult<UserDto>>.Ok(result);
    }

    /// <summary>HTTP GET - 根据主键获取单条详情。</summary>
    [Permission("sys:user:list")]
    [HttpGet("get")]
    public async Task<ApiResult<UserDto?>> Get([FromQuery] string userCode)
    {
        var user = await _userService.GetAsync(userCode);

        if (user == null) return ApiResult<UserDto?>.NotFound("用户不存在");

        return ApiResult<UserDto?>.Ok(user);
    }

    /// <summary>HTTP POST - 新增或更新实体信息。</summary>
    [Permission("sys:user:edit")]
    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] UserSaveDto dto) => await _userService.SaveAsync(dto);

    /// <summary>HTTP POST - 删除指定实体（软删或硬删）。</summary>
    [Permission("sys:user:delete")]
    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteRequest request) => await _userService.DeleteAsync(request.UserCode);
}

/// <summary>通用删除请求 DTO。</summary>

public class DeleteRequest { public string UserCode { get; set; } = string.Empty; }
