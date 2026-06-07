using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/msg")]
public class MsgController : ControllerBase
{
    private readonly MsgService _msgService;
    public MsgController(MsgService msgService) => _msgService = msgService;

    [Permission("sys:msg")]
    [HttpPost("send")]
    public async Task<ApiResult> Send([FromBody] MsgInnerSaveDto dto) => await _msgService.SendAsync(dto);

    [Permission("sys:msg")]
    [HttpGet("inbox")]
    public async Task<ApiResult<PageResult<MsgInnerRecordDto>>> Inbox([FromQuery] PageRequest request)
    {
        var userCode = User.FindFirst("UserCode")?.Value ?? "";
        return ApiResult<PageResult<MsgInnerRecordDto>>.Ok(await _msgService.GetInboxAsync(userCode, request));
    }

    [Permission("sys:msg")]
    [HttpGet("sent")]
    public async Task<ApiResult<PageResult<MsgInnerDto>>> Sent([FromQuery] PageRequest request)
    {
        var userCode = User.FindFirst("UserCode")?.Value ?? "";
        return ApiResult<PageResult<MsgInnerDto>>.Ok(await _msgService.GetSentAsync(userCode, request));
    }

    [Permission("sys:msg")]
    [HttpGet("unread-count")]
    public async Task<ApiResult<int>> UnreadCount()
    {
        var userCode = User.FindFirst("UserCode")?.Value ?? "";
        return ApiResult<int>.Ok(await _msgService.GetUnreadCountAsync(userCode));
    }

    [Permission("sys:msg")]
    [HttpPost("read/{recordId}")]
    public async Task<ApiResult> MarkRead(string recordId) => await _msgService.MarkReadAsync(recordId);

    [Permission("sys:msg")]
    [HttpDelete("{id}")]
    public async Task<ApiResult> Delete(string id) => await _msgService.DeleteAsync(id);

    [Permission("sys:msg")]
    [HttpGet("template/{tplKey}")]
    public async Task<ApiResult<MsgTemplateDto?>> GetTemplate(string tplKey)
    {
        var dto = await _msgService.GetTemplateAsync(tplKey);
        return dto == null ? ApiResult<MsgTemplateDto?>.NotFound("模板不存在") : ApiResult<MsgTemplateDto?>.Ok(dto);
    }

    [Permission("sys:msg")]
    [HttpPost("template")]
    public async Task<ApiResult> SaveTemplate([FromBody] MsgTemplateSaveDto dto) => await _msgService.SaveTemplateAsync(dto);
}
