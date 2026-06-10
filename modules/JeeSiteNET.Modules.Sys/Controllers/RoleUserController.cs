using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/role")]
public class RoleUserController : ControllerBase
{
    private readonly IUserRoleRepository _userRoleRepo;
    private readonly IUserRepository _userRepo;

    public RoleUserController(IUserRoleRepository userRoleRepo, IUserRepository userRepo)
    {
        _userRoleRepo = userRoleRepo;
        _userRepo = userRepo;
    }

    [Permission("sys:role:user")]
    [HttpGet("{roleCode}/users")]
    public async Task<ApiResult<List<User>>> GetUsersByRole(string roleCode)
    {
        var userCodes = await _userRoleRepo.GetUserCodesByRoleAsync(roleCode);
        var allUsers = await _userRepo.FindListAsync();
        var users = allUsers.Where(u => userCodes.Contains(u.UserCode)).ToList();
        return ApiResult<List<User>>.Ok(users);
    }

    [Permission("sys:role:user")]
    [HttpPost("{roleCode}/save-users")]
    public async Task<ApiResult> SaveUsers(string roleCode, [FromBody] List<string> userCodes)
    {
        await _userRoleRepo.SaveRoleUsersAsync(roleCode, userCodes);
        return ApiResult.Ok("保存成功");
    }

    [Permission("sys:role:user")]
    [HttpPost("{roleCode}/delete-user/{userCode}")]
    public async Task<ApiResult> DeleteUser(string roleCode, string userCode)
    {
        var userCodes = await _userRoleRepo.GetUserCodesByRoleAsync(roleCode);
        userCodes.Remove(userCode);
        await _userRoleRepo.SaveRoleUsersAsync(roleCode, userCodes);
        return ApiResult.Ok("已移除");
    }

    [Permission("sys:role:user")]
    [HttpGet("available-users")]
    public async Task<ApiResult<List<User>>> GetAvailableUsers([FromQuery] string? roleCode = null)
    {
        var users = await _userRepo.FindListAsync();
        if (!string.IsNullOrEmpty(roleCode))
        {
            var assigned = await _userRoleRepo.GetUserCodesByRoleAsync(roleCode);
            users = users.Where(u => !assigned.Contains(u.UserCode)).ToList();
        }
        return ApiResult<List<User>>.Ok(users);
    }
}
