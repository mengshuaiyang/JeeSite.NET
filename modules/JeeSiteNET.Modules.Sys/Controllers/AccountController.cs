using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Core.Utils;
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
    [HttpPost("password-question-by-login")]
    public async Task<ApiResult<object?>> GetPasswordQuestion([FromBody] GetPwdQuestionByLoginDto dto)
    {
        var user = await _userRepo.GetByLoginCodeAsync(dto.LoginCode);
        if (user == null) return ApiResult<object?>.NotFound("账号不存在");
        if (string.IsNullOrEmpty(user.PwdQuestion))
            return ApiResult<object?>.Ok(new { hasQuestion = false });
        return ApiResult<object?>.Ok(new { hasQuestion = true, question = user.PwdQuestion });
    }

    [AllowAnonymous]
    [HttpPost("reset-password-by-question")]
    public async Task<ApiResult> ResetPasswordByQuestion([FromBody] ResetPwdByQuestionDto dto)
    {
        var user = await _userRepo.GetByLoginCodeAsync(dto.LoginCode);
        if (user == null) return ApiResult.NotFound("账号不存在");
        if (string.IsNullOrEmpty(user.PwdQuestion) || string.IsNullOrEmpty(user.PwdQuestionAnswer))
            return ApiResult.Fail(400, "未设置安全问题");
        if (!string.Equals(user.PwdQuestionAnswer, dto.Answer, StringComparison.OrdinalIgnoreCase))
            return ApiResult.Fail(400, "安全问题的答案不正确");

        var now = DateTime.Now;
        user.Password = EncryptUtil.Md5(dto.NewPassword);
        user.PwdSecurityLevel = PasswordStrengthUtil.Evaluate(dto.NewPassword);
        user.PwdUpdateDate = now;
        user.PwdUpdateRecord = EncryptUtil.Md5(dto.NewPassword);
        await _userRepo.UpdateAsync(user);
        return ApiResult.Ok("密码重置成功");
    }

    [AllowAnonymous]
    [HttpPost("login-by-valid-code")]
    public async Task<ApiResult> LoginByValidCode([FromBody] LoginByCodeDto dto)
    {
        // Validate the verification code and login
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

public class GetPwdQuestionByLoginDto
{
    public string LoginCode { get; set; } = string.Empty;
}

public class ResetPwdByQuestionDto
{
    public string LoginCode { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
