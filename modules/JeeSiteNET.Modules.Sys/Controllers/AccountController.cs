using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
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
    private readonly ValidCodeService _validCode;
    private readonly AuthService _authService;

    public AccountController(IUserRepository userRepo, ICurrentUser currentUser, ValidCodeService validCode, AuthService authService)
    {
        _userRepo = userRepo;
        _currentUser = currentUser;
        _validCode = validCode;
        _authService = authService;
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
    [HttpPost("send-valid-code")]
    public async Task<ApiResult> SendValidCode([FromBody] SendValidCodeDto dto)
        => await _validCode.GenerateAndSendAsync(dto.Target, dto.Scene);

    [AllowAnonymous]
    [HttpPost("verify-valid-code")]
    public async Task<ApiResult> VerifyValidCode([FromBody] VerifyValidCodeDto dto)
        => await _validCode.VerifyAsync(dto.Target, dto.Scene, dto.Code);

    [AllowAnonymous]
    [HttpPost("login-by-valid-code")]
    public async Task<ApiResult<LoginResultDto>> LoginByValidCode([FromBody] LoginByCodeDto dto)
        => await _authService.LoginByCodeAsync(dto.Target, dto.Code);

    [AllowAnonymous]
    [HttpPost("reset-password-by-code")]
    public async Task<ApiResult> ResetPasswordByCode([FromBody] ResetPwdByCodeDto dto)
    {
        var valid = await _validCode.VerifySilentAsync(dto.Target, "reset", dto.Code);
        if (!valid) return ApiResult.Fail(400, "验证码错误或已过期");

        Domain.Entities.User? user;
        if (dto.Target.Contains('@'))
            user = await _userRepo.GetByEmailAsync(dto.Target);
        else
            user = await _userRepo.GetByPhoneAsync(dto.Target);

        if (user == null) return ApiResult.NotFound("该手机号/邮箱未注册账号");

        var now = DateTime.Now;
        user.Password = EncryptUtil.Md5(dto.NewPassword);
        user.PwdSecurityLevel = PasswordStrengthUtil.Evaluate(dto.NewPassword);
        user.PwdUpdateDate = now;
        user.PwdUpdateRecord = EncryptUtil.Md5(dto.NewPassword);
        await _userRepo.UpdateAsync(user);
        return ApiResult.Ok("密码重置成功");
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

public class SendValidCodeDto
{
    public string Target { get; set; } = string.Empty;
    public string Scene { get; set; } = "login";
}

public class VerifyValidCodeDto
{
    public string Target { get; set; } = string.Empty;
    public string Scene { get; set; } = "login";
    public string Code { get; set; } = string.Empty;
}

public class LoginByCodeDto
{
    public string Target { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}

public class ResetPwdByCodeDto
{
    public string Target { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
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
