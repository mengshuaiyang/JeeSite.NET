    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Modules.App.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.App.Application.DTOs
using JeeSiteNET.Modules.App.Application.DTOs;
    // 引入 JeeSiteNET.Modules.App.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.App.Application.Services
using JeeSiteNET.Modules.App.Application.Services;
    // 引入 Microsoft.AspNetCore.Authorization 命名空间
// 引入命名空间：Microsoft.AspNetCore.Authorization
using Microsoft.AspNetCore.Authorization;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.App.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.App.Controllers
namespace JeeSiteNET.Modules.App.Controllers;

[ApiController]
[Route("api/v1/app")]
// 定义class AppController
// 定义类：AppController

public class AppController : ControllerBase
{
    // 字段 _appService
    // 字段：_appService

    private readonly AppService _appService;
    // 构造函数 AppController
    // 构造函数：AppController

    public AppController(AppService appService) => _appService = appService;

    // --- Feedback ---

    [Permission("app:comment:list")]
    [HttpGet("comment/list")]
    // 方法 CommentList
    // 方法：CommentList

    public async Task<ApiResult<List<AppCommentDto>>> CommentList()
        => ApiResult<List<AppCommentDto>>.Ok(await _appService.GetCommentsAsync());

    [Permission("app:comment:save")]
    [HttpPost("comment/save")]
    // 方法 CommentSave
    // 方法：CommentSave

    public async Task<ApiResult> CommentSave([FromBody] AppCommentSaveDto dto)
        => await _appService.SaveCommentAsync(dto);

    [Permission("app:comment:reply")]
    [HttpPost("comment/reply")]
    // 方法 CommentReply
    // 方法：CommentReply

    public async Task<ApiResult> CommentReply([FromQuery] string id, [FromQuery] string replyContent)
        => await _appService.ReplyCommentAsync(id, replyContent);

    [Permission("app:comment:delete")]
    [HttpPost("comment/delete")]
    // 方法 CommentDelete
    // 方法：CommentDelete

    public async Task<ApiResult> CommentDelete([FromBody] DeleteAppRequest request)
        => await _appService.DeleteCommentAsync(request.Id);

    // --- Upgrade ---

    [Permission("app:upgrade:list")]
    [HttpGet("upgrade/list")]
    // 方法 UpgradeList
    // 方法：UpgradeList

    public async Task<ApiResult<List<AppUpgradeDto>>> UpgradeList()
        => ApiResult<List<AppUpgradeDto>>.Ok(await _appService.GetUpgradesAsync());

    [AllowAnonymous]
    [HttpGet("upgrade/latest")]
    // 方法 UpgradeLatest
    // 方法：UpgradeLatest

    public async Task<ApiResult<AppUpgradeDto?>> UpgradeLatest([FromQuery] string? appCode)
        => ApiResult<AppUpgradeDto?>.Ok(await _appService.GetLatestUpgradeAsync(appCode));

    [Permission("app:upgrade:edit")]
    [HttpPost("upgrade/save")]
    // 方法 UpgradeSave
    // 方法：UpgradeSave

    public async Task<ApiResult> UpgradeSave([FromBody] AppUpgradeSaveDto dto)
        => await _appService.SaveUpgradeAsync(dto);

    [Permission("app:upgrade:delete")]
    [HttpPost("upgrade/delete")]
    // 方法 UpgradeDelete
    // 方法：UpgradeDelete

    public async Task<ApiResult> UpgradeDelete([FromBody] DeleteAppRequest request)
        => await _appService.DeleteUpgradeAsync(request.Id);
}

// 定义class DeleteAppRequest
// 定义类：DeleteAppRequest

public class DeleteAppRequest { public string Id { get; set; } = string.Empty; }
