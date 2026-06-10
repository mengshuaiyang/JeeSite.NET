using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/account")]
public class AccountController : ControllerBase
{
    private readonly IUserRepository _userRepo;
    private readonly ICurrentUser _currentUser;

    public AccountController(IUserRepository userRepo, ICurrentUser currentUser)
    {
        _userRepo = userRepo;
        _currentUser = currentUser;
    }

    [Permission("sys:account:view")]
    [HttpGet("security")]
    public async Task<ApiResult<object>> GetSecurityInfo()
    {
        var user = await _userRepo.GetAsync(_currentUser.UserCode);
        if (user == null) return ApiResult<object>.NotFound("用户不存在");
        return ApiResult<object>.Ok(new
        {
            user.PwdSecurityLevel,
            user.PwdUpdateDate,
            user.PwdQuestion,
            user.LoginDate,
            user.LoginIp,
            user.LoginCount,
            user.FreezeDate,
            user.FreezeCause
        });
    }

    [Permission("sys:account:edit")]
    [HttpPost("password-question")]
    public async Task<ApiResult> SetPasswordQuestion([FromBody] SetPwdQuestionDto dto)
    {
        var user = await _userRepo.GetAsync(_currentUser.UserCode);
        if (user == null) return ApiResult.NotFound("用户不存在");
        user.PwdQuestion = dto.Question;
        user.PwdQuestionAnswer = dto.Answer;
        await _userRepo.UpdateAsync(user);
        return ApiResult.Ok("保存成功");
    }

    [AllowAnonymous]
    [HttpPost("login-by-valid-code")]
    public async Task<ApiResult> LoginByValidCode([FromBody] LoginByCodeDto dto)
    {
        // Validate the verification code and login
        // This is a simplified version - in production, integrate with SMS/email service
        return ApiResult.Ok("功能实现中");
    }

    [Permission("sys:account:edit")]
    [HttpPost("unlock")]
    public async Task<ApiResult> UnlockUser([FromBody] string userCode)
    {
        var user = await _userRepo.GetAsync(userCode);
        if (user == null) return ApiResult.NotFound("用户不存在");
        user.FreezeDate = null;
        user.FreezeCause = null;
        await _userRepo.UpdateAsync(user);
        return ApiResult.Ok("已解锁");
    }
}

public class SetPwdQuestionDto
{
    public string? Question { get; set; }
    public string? Answer { get; set; }
}

public class LoginByCodeDto
{
    public string LoginCode { get; set; } = string.Empty;
    public string ValidCode { get; set; } = string.Empty;
}
