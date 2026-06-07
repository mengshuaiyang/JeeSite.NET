using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Cms.Application.DTOs;
using JeeSiteNET.Modules.Cms.Application.Services;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Cms.Controllers;

[ApiController]
[Route("api/v1/cms")]
public class CmsController : ControllerBase
{
    private readonly CmsService _cmsService;
    public CmsController(CmsService cmsService) => _cmsService = cmsService;

    // --- Comments ---

    [AllowAnonymous]
    [HttpPost("comment/list")]
    public async Task<ApiResult<PageResult<CommentDto>>> CommentList([FromBody] PageRequest<Comment> request)
        => ApiResult<PageResult<CommentDto>>.Ok(await _cmsService.FindCommentPageAsync(request));

    [AllowAnonymous]
    [HttpPost("comment/save")]
    public async Task<ApiResult> CommentSave([FromBody] CommentSaveDto dto)
        => await _cmsService.SaveCommentAsync(dto);

    [Permission("cms:comment:audit")]
    [HttpPost("comment/audit")]
    public async Task<ApiResult> CommentAudit([FromQuery] string commentCode, [FromQuery] string status, [FromQuery] string? auditComment)
        => await _cmsService.AuditCommentAsync(commentCode, status, auditComment);

    [Permission("cms:comment:delete")]
    [HttpPost("comment/delete")]
    public async Task<ApiResult> CommentDelete([FromBody] DeleteCommentRequest request)
        => await _cmsService.DeleteCommentAsync(request.CommentCode);

    // --- Guestbook ---

    [AllowAnonymous]
    [HttpPost("guestbook/list")]
    public async Task<ApiResult<PageResult<GuestbookDto>>> GuestbookList([FromBody] PageRequest<Guestbook> request)
        => ApiResult<PageResult<GuestbookDto>>.Ok(await _cmsService.FindGuestbookPageAsync(request));

    [AllowAnonymous]
    [HttpPost("guestbook/save")]
    public async Task<ApiResult> GuestbookSave([FromBody] GuestbookSaveDto dto)
        => await _cmsService.SaveGuestbookAsync(dto);

    [Permission("cms:guestbook:reply")]
    [HttpPost("guestbook/reply")]
    public async Task<ApiResult> GuestbookReply([FromQuery] string gbCode, [FromQuery] string reContent)
        => await _cmsService.ReplyGuestbookAsync(gbCode, reContent);

    [Permission("cms:guestbook:delete")]
    [HttpPost("guestbook/delete")]
    public async Task<ApiResult> GuestbookDelete([FromBody] DeleteGuestbookRequest request)
        => await _cmsService.DeleteGuestbookAsync(request.GbCode);

    // --- Tags ---

    [AllowAnonymous]
    [HttpGet("tag/list")]
    public async Task<ApiResult<List<TagDto>>> TagList()
        => ApiResult<List<TagDto>>.Ok(await _cmsService.GetAllTagsAsync());

    [Permission("cms:tag:save")]
    [HttpPost("tag/save")]
    public async Task<ApiResult> TagSave([FromQuery] string tagName)
        => await _cmsService.SaveTagAsync(tagName);

    [Permission("cms:tag:delete")]
    [HttpPost("tag/delete")]
    public async Task<ApiResult> TagDelete([FromBody] DeleteTagRequest request)
        => await _cmsService.DeleteTagAsync(request.TagName);

    // --- Visit Log ---

    [HttpPost("visit-log/add")]
    public async Task<ApiResult> VisitLogAdd([FromBody] VisitLog log)
        => await _cmsService.AddVisitLogAsync(log);

    [Permission("cms:visit-log:list")]
    [HttpPost("visit-log/list")]
    public async Task<ApiResult<PageResult<VisitLogDto>>> VisitLogList([FromBody] PageRequest<VisitLog> request)
        => ApiResult<PageResult<VisitLogDto>>.Ok(await _cmsService.FindVisitLogPageAsync(request));

    [AllowAnonymous]
    [HttpGet("stats/today-visits")]
    public async Task<ApiResult<long>> TodayVisits()
        => ApiResult<long>.Ok(await _cmsService.GetTodayVisitCountAsync());
}

public class DeleteCommentRequest { public string CommentCode { get; set; } = string.Empty; }
public class DeleteGuestbookRequest { public string GbCode { get; set; } = string.Empty; }
public class DeleteTagRequest { public string TagName { get; set; } = string.Empty; }
