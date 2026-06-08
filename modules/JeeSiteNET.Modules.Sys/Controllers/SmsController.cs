using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/sms")]
public class SmsController : ControllerBase
{
    private readonly ISmsSender _smsSender;

    public SmsController(ISmsSender smsSender) => _smsSender = smsSender;

    [HttpPost("send")]
    [AllowAnonymous]
    public async Task<ApiResult<string>> Send([FromBody] SmsDto dto)
    {
        if (string.IsNullOrEmpty(dto.PhoneNumber))
            return ApiResult<string>.Fail(400, "手机号不能为空");
        if (string.IsNullOrEmpty(dto.Message))
            return ApiResult<string>.Fail(400, "短信内容不能为空");

        var ok = await _smsSender.SendAsync(dto.PhoneNumber, dto.Message);
        return ok ? ApiResult<string>.Ok("发送成功") : ApiResult<string>.Fail(500, "发送失败");
    }
}
