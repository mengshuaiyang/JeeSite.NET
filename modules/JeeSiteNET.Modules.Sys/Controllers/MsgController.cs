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
[Route("api/v1/sys/msg")]
// 定义class MsgController
// 定义类：MsgController

public class MsgController : ControllerBase
{
    // 字段 _msgService
    // 字段：_msgService

    private readonly MsgService _msgService;
    // 构造函数 MsgController
    // 构造函数：MsgController

    public MsgController(MsgService msgService) => _msgService = msgService;

    [Permission("sys:msg")]
    [HttpPost("send")]
    // 方法：Send

    public async Task<ApiResult> Send([FromBody] MsgInnerSaveDto dto) => await _msgService.SendAsync(dto);

    [Permission("sys:msg")]
    [HttpGet("inbox")]
    // 方法 Inbox
    // 方法：Inbox

    public async Task<ApiResult<PageResult<MsgInnerRecordDto>>> Inbox([FromQuery] PageRequest request)
    {
        // 声明并初始化变量：userCode
        var userCode = User.FindFirst("UserCode")?.Value ?? "";
        // return 返回结果
        return ApiResult<PageResult<MsgInnerRecordDto>>.Ok(await _msgService.GetInboxAsync(userCode, request));
    }

    [Permission("sys:msg")]
    [HttpGet("sent")]
    // 方法 Sent
    // 方法：Sent

    public async Task<ApiResult<PageResult<MsgInnerDto>>> Sent([FromQuery] PageRequest request)
    {
        // 声明并初始化变量：userCode
        var userCode = User.FindFirst("UserCode")?.Value ?? "";
        // return 返回结果
        return ApiResult<PageResult<MsgInnerDto>>.Ok(await _msgService.GetSentAsync(userCode, request));
    }

    [Permission("sys:msg")]
    [HttpGet("unread-count")]
    // 方法 UnreadCount
    // 方法：UnreadCount

    public async Task<ApiResult<int>> UnreadCount()
    {
        // 声明并初始化变量：userCode
        var userCode = User.FindFirst("UserCode")?.Value ?? "";
        // return 返回结果
        return ApiResult<int>.Ok(await _msgService.GetUnreadCountAsync(userCode));
    }

    [Permission("sys:msg")]
    [HttpPost("read/{recordId}")]
    // 方法：MarkRead

    public async Task<ApiResult> MarkRead(string recordId) => await _msgService.MarkReadAsync(recordId);

    [Permission("sys:msg")]
    [HttpDelete("{id}")]
    // 方法：Delete

    public async Task<ApiResult> Delete(string id) => await _msgService.DeleteAsync(id);

    [Permission("sys:msg")]
    [HttpGet("template/{tplKey}")]
    // 方法 GetTemplate
    // 方法：GetTemplate

    public async Task<ApiResult<MsgTemplateDto?>> GetTemplate(string tplKey)
    {
        var dto = await _msgService.GetTemplateAsync(tplKey);
        // return 返回结果
        return dto == null ? ApiResult<MsgTemplateDto?>.NotFound("模板不存在") : ApiResult<MsgTemplateDto?>.Ok(dto);
    }

    [Permission("sys:msg")]
    [HttpPost("template")]
    // 方法：SaveTemplate

    public async Task<ApiResult> SaveTemplate([FromBody] MsgTemplateSaveDto dto) => await _msgService.SaveTemplateAsync(dto);
}
