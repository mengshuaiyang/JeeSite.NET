using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZiggyCreatures.Caching.Fusion;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IFusionCache _cache;

    public ProfileController(IUserRepository userRepository, ICurrentUser currentUser, IFusionCache cache)
    {
        _userRepository = userRepository;
        _currentUser = currentUser;
        _cache = cache;
    }

    [HttpGet]
    public async Task<ApiResult<UserDto>> Get()
    {
        var user = await _userRepository.GetAsync(_currentUser.UserCode);
        if (user == null) return ApiResult<UserDto>.NotFound("用户不存在");
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
    public async Task<ApiResult> Update([FromBody] ProfileUpdateDto dto)
    {
        var user = await _userRepository.GetAsync(_currentUser.UserCode);
        if (user == null) return ApiResult.Fail(404, "用户不存在");
        if (!string.IsNullOrEmpty(dto.UserName)) user.UserName = dto.UserName;
        if (!string.IsNullOrEmpty(dto.Email)) user.Email = dto.Email;
        if (!string.IsNullOrEmpty(dto.Phone)) user.Phone = dto.Phone;
        await _userRepository.UpdateAsync(user);
        return ApiResult.Ok();
    }

    [HttpPost("password")]
    public async Task<ApiResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        if (string.IsNullOrEmpty(dto.OldPassword) || string.IsNullOrEmpty(dto.NewPassword))
            return ApiResult.Fail(400, "旧密码和新密码不能为空");
        if (dto.NewPassword.Length < 6)
            return ApiResult.Fail(400, "新密码至少6位");

        var user = await _userRepository.GetAsync(_currentUser.UserCode);
        if (user == null) return ApiResult.Fail(404, "用户不存在");

        var oldMd5 = EncryptUtil.Md5(dto.OldPassword);
        if (user.Password != oldMd5) return ApiResult.Fail(400, "旧密码不正确");

        user.Password = EncryptUtil.Md5(dto.NewPassword);
        user.PwdUpdateDate = DateTime.Now;
        await _userRepository.UpdateAsync(user);
        return ApiResult.Ok();
    }

    [HttpGet("desktop")]
    public async Task<ApiResult<object>> Desktop()
    {
        var user = await _userRepository.GetAsync(_currentUser.UserCode);
        if (user == null) return ApiResult<object>.NotFound("用户不存在");

        var roleCodes = await _cache.GetOrSetAsync($"DesktopRoles:{_currentUser.UserCode}", async ct =>
        {
            return new List<string>();
        }, TimeSpan.FromMinutes(5));

        return ApiResult<object>.Ok(new
        {
            user.LoginCode,
            user.UserName,
            user.LoginDate,
            user.LoginCount,
            user.PwdUpdateDate,
            user.PwdSecurityLevel,
            PwdQuestionSet = !string.IsNullOrEmpty(user.PwdQuestion),
            Avatar = user.Avatar ?? "/avatars/default.png",
            Stats = new
            {
                MenuCount = 0,
                RoleCount = 0,
                LoginDays = (DateTime.Now - (user.CreateDate ?? DateTime.Now)).Days
            }
        });
    }

    [HttpPost("avatar")]
    public async Task<ApiResult<string>> UpdateAvatar([FromBody] AvatarUpdateDto dto)
    {
        var user = await _userRepository.GetAsync(_currentUser.UserCode);
        if (user == null) return ApiResult<string>.Fail(404, "用户不存在");
        user.Avatar = dto.AvatarUrl;
        await _userRepository.UpdateAsync(user);
        return ApiResult<string>.Ok(dto.AvatarUrl);
    }
}

public class ProfileUpdateDto
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Sex { get; set; }
    public string? Sign { get; set; }
}

public class ChangePasswordDto
{
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

public class AvatarUpdateDto
{
    public string AvatarUrl { get; set; } = string.Empty;
}
