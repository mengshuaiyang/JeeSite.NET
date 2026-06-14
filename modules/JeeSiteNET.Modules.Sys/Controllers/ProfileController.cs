    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Core.Utils 命名空间
// 引入命名空间：JeeSiteNET.Core.Utils
using JeeSiteNET.Core.Utils;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 Microsoft.AspNetCore.Authorization 命名空间
// 引入命名空间：Microsoft.AspNetCore.Authorization
using Microsoft.AspNetCore.Authorization;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;
    // 引入 ZiggyCreatures.Caching.Fusion 命名空间
// 引入命名空间：ZiggyCreatures.Caching.Fusion
using ZiggyCreatures.Caching.Fusion;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/profile")]
[Authorize]
// 定义class ProfileController
// 定义类：ProfileController

public class ProfileController : ControllerBase
{
    // 字段 _userRepository
    // 字段：_userRepository

    private readonly IUserRepository _userRepository;
    // 字段 _currentUser
    // 字段：_currentUser

    private readonly ICurrentUser _currentUser;
    // 字段 _cache
    // 字段：_cache

    private readonly IFusionCache _cache;

    // 方法 ProfileController
    // 构造函数：ProfileController

    public ProfileController(IUserRepository userRepository, ICurrentUser currentUser, IFusionCache cache)
    {
        _userRepository = userRepository;
        _currentUser = currentUser;
        _cache = cache;
    }

    [HttpGet]
    // 方法 Get
    // 方法：Get

    public async Task<ApiResult<UserDto>> Get()
    {
        // 缓存：获取值
        var user = await _userRepository.GetAsync(_currentUser.UserCode);
        // if 条件判断
        if (user == null) return ApiResult<UserDto>.NotFound("用户不存在");
        // return 返回结果
        return ApiResult<UserDto>.Ok(new UserDto
        {
            UserCode = user.UserCode, LoginCode = user.LoginCode,
            UserName = user.UserName, UserType = user.UserType,
            Avatar = user.Avatar, Email = user.Email, Phone = user.Phone,
            OrgCode = user.OrgCode, OrgName = user.OrgName,
            LoginDate = user.LoginDate, CreateDate = user.CreateDate
        });
    }

    [HttpPost("update")]
    // 方法 Update
    // 方法：Update

    public async Task<ApiResult> Update([FromBody] ProfileUpdateDto dto)
    {
        // 缓存：获取值
        var user = await _userRepository.GetAsync(_currentUser.UserCode);
        // if 条件判断
        if (user == null) return ApiResult.Fail(404, "用户不存在");
        // if 条件判断
        if (!string.IsNullOrEmpty(dto.UserName)) user.UserName = dto.UserName;
        // if 条件判断
        if (!string.IsNullOrEmpty(dto.Email)) user.Email = dto.Email;
        // if 条件判断
        if (!string.IsNullOrEmpty(dto.Phone)) user.Phone = dto.Phone;
        // await 异步等待
        await _userRepository.UpdateAsync(user);
        // return 返回结果
        return ApiResult.Ok();
    }

    [HttpPost("password")]
    // 方法 ChangePassword
    // 方法：ChangePassword

    public async Task<ApiResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        // if 条件判断
        if (string.IsNullOrEmpty(dto.OldPassword) || string.IsNullOrEmpty(dto.NewPassword))
            // return 返回结果
            return ApiResult.Fail(400, "旧密码和新密码不能为空");
        // if 条件判断
        if (dto.NewPassword.Length < 6)
            // return 返回结果
            return ApiResult.Fail(400, "新密码至少6位");

        // 缓存：获取值
        var user = await _userRepository.GetAsync(_currentUser.UserCode);
        // if 条件判断
        if (user == null) return ApiResult.Fail(404, "用户不存在");

        // 声明并初始化变量：oldMd5
        var oldMd5 = EncryptUtil.Md5(dto.OldPassword);
        // if 条件判断
        if (user.Password != oldMd5) return ApiResult.Fail(400, "旧密码不正确");

        user.Password = EncryptUtil.Md5(dto.NewPassword);
        user.PwdUpdateDate = DateTime.Now;
        // await 异步等待
        await _userRepository.UpdateAsync(user);
        // return 返回结果
        return ApiResult.Ok();
    }

    [HttpGet("desktop")]
    // 方法 Desktop
    // 方法：Desktop

    public async Task<ApiResult<object>> Desktop()
    {
        // 缓存：获取值
        var user = await _userRepository.GetAsync(_currentUser.UserCode);
        // if 条件判断
        if (user == null) return ApiResult<object>.NotFound("用户不存在");

        var roleCodes = await _cache.GetOrSetAsync($"DesktopRoles:{_currentUser.UserCode}", async ct =>
        {
            // For simplicity, return role info from user
            // return 返回结果
            return new List<string>();
        }, TimeSpan.FromMinutes(5));

        // return 返回结果
        return ApiResult<object>.Ok(new
        {
            user.LoginCode,
            user.UserName,
            user.LoginDate,
            user.LoginCount,
            user.PwdUpdateDate,
            user.PwdSecurityLevel,
            // 调用 IsNullOrEmpty
            PwdQuestionSet = !string.IsNullOrEmpty(user.PwdQuestion),
            // null 合并操作 ??（若为 null 则使用右侧值）
            Avatar = user.Avatar ?? "/avatars/default.png",
            Stats = new
            {
                MenuCount = 0,
                RoleCount = 0,
                // null 合并操作 ??（若为 null 则使用右侧值）
                LoginDays = (DateTime.Now - (user.CreateDate ?? DateTime.Now)).Days
            }
        });
    }

    [HttpPost("avatar")]
    // 方法 UpdateAvatar
    // 方法：UpdateAvatar

    public async Task<ApiResult<string>> UpdateAvatar([FromBody] AvatarUpdateDto dto)
    {
        // 缓存：获取值
        var user = await _userRepository.GetAsync(_currentUser.UserCode);
        // if 条件判断
        if (user == null) return ApiResult<string>.Fail(404, "用户不存在");
        user.Avatar = dto.AvatarUrl;
        // await 异步等待
        await _userRepository.UpdateAsync(user);
        // return 返回结果
        return ApiResult<string>.Ok(dto.AvatarUrl);
    }
}

// 定义class ProfileUpdateDto
// 定义类：ProfileUpdateDto

public class ProfileUpdateDto
{
    // 属性：UserName

    public string? UserName { get; set; }
    // 属性：Email

    public string? Email { get; set; }
    // 属性：Phone

    public string? Phone { get; set; }
    // 属性：Sex

    public string? Sex { get; set; }
    // 属性：Sign

    public string? Sign { get; set; }
}

// 定义class ChangePasswordDto
// 定义类：ChangePasswordDto

public class ChangePasswordDto
{
    // 属性 OldPassword
    // 属性：OldPassword

    public string OldPassword { get; set; } = string.Empty;
    // 属性 NewPassword
    // 属性：NewPassword

    public string NewPassword { get; set; } = string.Empty;
}

// 定义class AvatarUpdateDto
// 定义类：AvatarUpdateDto

public class AvatarUpdateDto
{
    // 属性 AvatarUrl
    // 属性：AvatarUrl

    public string AvatarUrl { get; set; } = string.Empty;
}
