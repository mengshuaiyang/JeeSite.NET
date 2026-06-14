    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/role")]
// 定义class RoleUserController
// 定义类：RoleUserController

public class RoleUserController : ControllerBase
{
    // 字段 _userRoleRepo
    // 字段：_userRoleRepo

    private readonly IUserRoleRepository _userRoleRepo;
    // 字段 _userRepo
    // 字段：_userRepo

    private readonly IUserRepository _userRepo;

    // 方法 RoleUserController
    // 构造函数：RoleUserController

    public RoleUserController(IUserRoleRepository userRoleRepo, IUserRepository userRepo)
    {
        _userRoleRepo = userRoleRepo;
        _userRepo = userRepo;
    }

    [Permission("sys:role:user")]
    [HttpGet("{roleCode}/users")]
    // 方法 GetUsersByRole
    // 方法：GetUsersByRole

    public async Task<ApiResult<List<User>>> GetUsersByRole(string roleCode)
    {
        var userCodes = await _userRoleRepo.GetUserCodesByRoleAsync(roleCode);
        var allUsers = await _userRepo.FindListAsync();
        // 集合操作：检查是否包含
        var users = allUsers.Where(u => userCodes.Contains(u.UserCode)).ToList();
        // return 返回结果
        return ApiResult<List<User>>.Ok(users);
    }

    [Permission("sys:role:user")]
    [HttpPost("{roleCode}/save-users")]
    // 方法 SaveUsers
    // 方法：SaveUsers

    public async Task<ApiResult> SaveUsers(string roleCode, [FromBody] List<string> userCodes)
    {
        // await 异步等待
        await _userRoleRepo.SaveRoleUsersAsync(roleCode, userCodes);
        // return 返回结果
        return ApiResult.Ok("保存成功");
    }

    [Permission("sys:role:user")]
    [HttpPost("{roleCode}/delete-user/{userCode}")]
    // 方法 DeleteUser
    // 方法：DeleteUser

    public async Task<ApiResult> DeleteUser(string roleCode, string userCode)
    {
        var userCodes = await _userRoleRepo.GetUserCodesByRoleAsync(roleCode);
        // 集合操作：移除元素
        userCodes.Remove(userCode);
        // await 异步等待
        await _userRoleRepo.SaveRoleUsersAsync(roleCode, userCodes);
        // return 返回结果
        return ApiResult.Ok("已移除");
    }

    [Permission("sys:role:user")]
    [HttpGet("available-users")]
    // 方法 GetAvailableUsers
    // 方法：GetAvailableUsers

    public async Task<ApiResult<List<User>>> GetAvailableUsers([FromQuery] string? roleCode = null)
    {
        var users = await _userRepo.FindListAsync();
        // if 条件判断
        if (!string.IsNullOrEmpty(roleCode))
        {
            var assigned = await _userRoleRepo.GetUserCodesByRoleAsync(roleCode);
            // 集合操作：检查是否包含
            users = users.Where(u => !assigned.Contains(u.UserCode)).ToList();
        }
        // return 返回结果
        return ApiResult<List<User>>.Ok(users);
    }
}
