using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/user")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    public UserController(UserService userService) => _userService = userService;

    [Permission("sys:user:list")]
    [HttpPost("list")]
    public async Task<ApiResult<PageResult<UserDto>>> List([FromBody] PageRequest<User> request)
    {
        var result = await _userService.FindPageAsync(request);
        return ApiResult<PageResult<UserDto>>.Ok(result);
    }

    [Permission("sys:user:list")]
    [HttpGet("get")]
    public async Task<ApiResult<UserDto?>> Get([FromQuery] string userCode)
    {
        var user = await _userService.GetAsync(userCode);
        if (user == null) return ApiResult<UserDto?>.NotFound("用户不存在");
        return ApiResult<UserDto?>.Ok(user);
    }

    [Permission("sys:user:edit")]
    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] UserSaveDto dto) => await _userService.SaveAsync(dto);

    [Permission("sys:user:delete")]
    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteRequest request) => await _userService.DeleteAsync(request.UserCode);
}

public class DeleteRequest { public string UserCode { get; set; } = string.Empty; }
