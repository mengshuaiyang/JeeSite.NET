    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services
using JeeSiteNET.Modules.Sys.Application.Services;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/msg/push")]
// 定义class MsgPushController
// 定义类：MsgPushController

public class MsgPushController : ControllerBase
{
    // 字段 _msgService
    // 字段：_msgService

    private readonly MsgService _msgService;

    // 构造函数 MsgPushController
    // 构造函数：MsgPushController

    public MsgPushController(MsgService msgService) => _msgService = msgService;

    [Permission("sys:msg:push:list")]
    [HttpPost("list")]
    // 方法：List

    public async Task<ApiResult<PageResult<MsgPushDto>>> List([FromBody] PageRequest request)
        => ApiResult<PageResult<MsgPushDto>>.Ok(await _msgService.GetPushListAsync(request));

    [Permission("sys:msg:push:edit")]
    [HttpPost("send")]
    // 方法 Send
    // 方法：Send

    public async Task<ApiResult> Send([FromBody] MsgPushSaveDto dto)
        => await _msgService.SendPushAsync(dto);

    [Permission("sys:msg:push:edit")]
    [HttpPost("retry")]
    // 方法 Retry
    // 方法：Retry

    public async Task<ApiResult> Retry([FromBody] RetryPushRequest request)
        => await _msgService.RetryPushAsync(request.Id);
}

// 定义class RetryPushRequest
// 定义类：RetryPushRequest

public class RetryPushRequest { public string Id { get; set; } = string.Empty; }
