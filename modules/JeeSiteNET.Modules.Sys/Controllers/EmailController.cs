    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
    // 引入 Microsoft.AspNetCore.Authorization 命名空间
// 引入命名空间：Microsoft.AspNetCore.Authorization
using Microsoft.AspNetCore.Authorization;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/email")]
// 定义class EmailController
// 定义类：EmailController

public class EmailController : ControllerBase
{
    // 字段 _emailSender
    // 字段：_emailSender

    private readonly IEmailSender _emailSender;

    // 构造函数 EmailController
    // 构造函数：EmailController

    public EmailController(IEmailSender emailSender) => _emailSender = emailSender;

    [HttpPost("send")]
    [AllowAnonymous]
    // 方法 Send
    // 方法：Send

    public async Task<ApiResult<string>> Send([FromBody] EmailDto dto)
    {
        // if 条件判断
        if (string.IsNullOrEmpty(dto.ToAddress))
            // return 返回结果
            return ApiResult<string>.Fail(400, "收件人不能为空");

        var ok = await _emailSender.SendAsync(dto.ToAddress, dto.Subject, dto.Body, dto.Cc, dto.Bcc);
        // return 返回结果
        return ok ? ApiResult<string>.Ok("发送成功") : ApiResult<string>.Fail(500, "发送失败");
    }
}
