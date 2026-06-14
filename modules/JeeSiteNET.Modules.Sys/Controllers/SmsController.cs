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
[Route("api/v1/sys/sms")]
// 定义class SmsController
// 定义类：SmsController

public class SmsController : ControllerBase
{
    // 字段 _smsSender
    // 字段：_smsSender

    private readonly ISmsSender _smsSender;

    // 构造函数 SmsController
    // 构造函数：SmsController

    public SmsController(ISmsSender smsSender) => _smsSender = smsSender;

    [HttpPost("send")]
    [AllowAnonymous]
    // 方法 Send
    // 方法：Send

    public async Task<ApiResult<string>> Send([FromBody] SmsDto dto)
    {
        // if 条件判断
        if (string.IsNullOrEmpty(dto.PhoneNumber))
            // return 返回结果
            return ApiResult<string>.Fail(400, "手机号不能为空");
        // if 条件判断
        if (string.IsNullOrEmpty(dto.Message))
            // return 返回结果
            return ApiResult<string>.Fail(400, "短信内容不能为空");

        var ok = await _smsSender.SendAsync(dto.PhoneNumber, dto.Message);
        // return 返回结果
        return ok ? ApiResult<string>.Ok("发送成功") : ApiResult<string>.Fail(500, "发送失败");
    }
}
