using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/msg/push")]
public class MsgPushController : ControllerBase
{
    private readonly MsgService _msgService;

    public MsgPushController(MsgService msgService) => _msgService = msgService;

    [Permission("sys:msg:push:list")]
    [HttpPost("list")]
    public async Task<ApiResult<PageResult<MsgPushDto>>> List([FromBody] PageRequest request)
        => ApiResult<PageResult<MsgPushDto>>.Ok(await _msgService.GetPushListAsync(request));

    [Permission("sys:msg:push:edit")]
    [HttpPost("send")]
    public async Task<ApiResult> Send([FromBody] MsgPushSaveDto dto)
        => await _msgService.SendPushAsync(dto);

    [Permission("sys:msg:push:edit")]
    [HttpPost("retry")]
    public async Task<ApiResult> Retry([FromBody] RetryPushRequest request)
        => await _msgService.RetryPushAsync(request.Id);
}

public class RetryPushRequest { public string Id { get; set; } = string.Empty; }
