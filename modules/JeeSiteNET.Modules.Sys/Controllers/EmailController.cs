using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/email")]
public class EmailController : ControllerBase
{
    private readonly IEmailSender _emailSender;

    public EmailController(IEmailSender emailSender) => _emailSender = emailSender;

    [HttpPost("send")]
    [AllowAnonymous]
    public async Task<ApiResult<string>> Send([FromBody] EmailDto dto)
    {
        if (string.IsNullOrEmpty(dto.ToAddress))
            return ApiResult<string>.Fail(400, "收件人不能为空");

        var ok = await _emailSender.SendAsync(dto.ToAddress, dto.Subject, dto.Body, dto.Cc, dto.Bcc);
        return ok ? ApiResult<string>.Ok("发送成功") : ApiResult<string>.Fail(500, "发送失败");
    }
}
