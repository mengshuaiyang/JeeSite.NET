using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.App.Application.DTOs;
using JeeSiteNET.Modules.App.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.App.Controllers;

[ApiController]
[Route("api/v1/app")]
public class AppController : ControllerBase
{
    private readonly AppService _appService;
    public AppController(AppService appService) => _appService = appService;

    // --- Feedback ---

    [Permission("app:comment:list")]
    [HttpGet("comment/list")]
    public async Task<ApiResult<List<AppCommentDto>>> CommentList()
        => ApiResult<List<AppCommentDto>>.Ok(await _appService.GetCommentsAsync());

    [Permission("app:comment:save")]
    [HttpPost("comment/save")]
    public async Task<ApiResult> CommentSave([FromBody] AppCommentSaveDto dto)
        => await _appService.SaveCommentAsync(dto);

    [Permission("app:comment:reply")]
    [HttpPost("comment/reply")]
    public async Task<ApiResult> CommentReply([FromQuery] string id, [FromQuery] string replyContent)
        => await _appService.ReplyCommentAsync(id, replyContent);

    [Permission("app:comment:delete")]
    [HttpPost("comment/delete")]
    public async Task<ApiResult> CommentDelete([FromBody] DeleteAppRequest request)
        => await _appService.DeleteCommentAsync(request.Id);

    // --- Upgrade ---

    [Permission("app:upgrade:list")]
    [HttpGet("upgrade/list")]
    public async Task<ApiResult<List<AppUpgradeDto>>> UpgradeList()
        => ApiResult<List<AppUpgradeDto>>.Ok(await _appService.GetUpgradesAsync());

    [AllowAnonymous]
    [HttpGet("upgrade/latest")]
    public async Task<ApiResult<AppUpgradeDto?>> UpgradeLatest([FromQuery] string? appCode)
        => ApiResult<AppUpgradeDto?>.Ok(await _appService.GetLatestUpgradeAsync(appCode));

    [Permission("app:upgrade:edit")]
    [HttpPost("upgrade/save")]
    public async Task<ApiResult> UpgradeSave([FromBody] AppUpgradeSaveDto dto)
        => await _appService.SaveUpgradeAsync(dto);

    [Permission("app:upgrade:delete")]
    [HttpPost("upgrade/delete")]
    public async Task<ApiResult> UpgradeDelete([FromBody] DeleteAppRequest request)
        => await _appService.DeleteUpgradeAsync(request.Id);
}

public class DeleteAppRequest { public string Id { get; set; } = string.Empty; }
