using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/user")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService) => _userService = userService;

    [HttpPost("list")]
    public async Task<ApiResult<PageResult<UserDto>>> List([FromBody] PageRequest<User> request)
    {
        var result = await _userService.FindPageAsync(request);
        return ApiResult<PageResult<UserDto>>.Ok(result);
    }

    [HttpGet("get")]
    public async Task<ApiResult<UserDto?>> Get([FromQuery] string userCode)
    {
        var user = await _userService.GetAsync(userCode);
        if (user == null) return ApiResult<UserDto?>.NotFound("用户不存在");
        return ApiResult<UserDto?>.Ok(user);
    }

    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] UserSaveDto dto)
    {
        return await _userService.SaveAsync(dto);
    }

    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteRequest request)
    {
        return await _userService.DeleteAsync(request.UserCode);
    }
}

public class DeleteRequest
{
    public string UserCode { get; set; } = string.Empty;
}
