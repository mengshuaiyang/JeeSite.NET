    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Modules.Cms.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Application.DTOs
using JeeSiteNET.Modules.Cms.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Cms.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Application.Services
using JeeSiteNET.Modules.Cms.Application.Services;
    // 引入 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
using JeeSiteNET.Modules.Cms.Domain.Entities;
    // 引入 Microsoft.AspNetCore.Authorization 命名空间
// 引入命名空间：Microsoft.AspNetCore.Authorization
using Microsoft.AspNetCore.Authorization;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Cms.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Controllers
namespace JeeSiteNET.Modules.Cms.Controllers;

[ApiController]
[Route("api/v1/cms")]
// 定义class CmsController
// 定义类：CmsController

public class CmsController : ControllerBase
{
    // 字段 _cmsService
    // 字段：_cmsService

    private readonly CmsService _cmsService;
    // 构造函数 CmsController
    // 构造函数：CmsController

    public CmsController(CmsService cmsService) => _cmsService = cmsService;

    // --- Comments ---

    [AllowAnonymous]
    [HttpPost("comment/list")]
    // 方法 CommentList
    // 方法：CommentList

    public async Task<ApiResult<PageResult<CommentDto>>> CommentList([FromBody] PageRequest<Comment> request)
        => ApiResult<PageResult<CommentDto>>.Ok(await _cmsService.FindCommentPageAsync(request));

    [AllowAnonymous]
    [HttpPost("comment/save")]
    // 方法 CommentSave
    // 方法：CommentSave

    public async Task<ApiResult> CommentSave([FromBody] CommentSaveDto dto)
        => await _cmsService.SaveCommentAsync(dto);

    [Permission("cms:comment:audit")]
    [HttpPost("comment/audit")]
    // 方法 CommentAudit
    // 方法：CommentAudit

    public async Task<ApiResult> CommentAudit([FromQuery] string commentCode, [FromQuery] string status, [FromQuery] string? auditComment)
        => await _cmsService.AuditCommentAsync(commentCode, status, auditComment);

    [Permission("cms:comment:delete")]
    [HttpPost("comment/delete")]
    // 方法 CommentDelete
    // 方法：CommentDelete

    public async Task<ApiResult> CommentDelete([FromBody] DeleteCommentRequest request)
        => await _cmsService.DeleteCommentAsync(request.CommentCode);

    // --- Guestbook ---

    [AllowAnonymous]
    [HttpPost("guestbook/list")]
    // 方法 GuestbookList
    // 方法：GuestbookList

    public async Task<ApiResult<PageResult<GuestbookDto>>> GuestbookList([FromBody] PageRequest<Guestbook> request)
        => ApiResult<PageResult<GuestbookDto>>.Ok(await _cmsService.FindGuestbookPageAsync(request));

    [AllowAnonymous]
    [HttpPost("guestbook/save")]
    // 方法 GuestbookSave
    // 方法：GuestbookSave

    public async Task<ApiResult> GuestbookSave([FromBody] GuestbookSaveDto dto)
        => await _cmsService.SaveGuestbookAsync(dto);

    [Permission("cms:guestbook:reply")]
    [HttpPost("guestbook/reply")]
    // 方法 GuestbookReply
    // 方法：GuestbookReply

    public async Task<ApiResult> GuestbookReply([FromQuery] string gbCode, [FromQuery] string reContent)
        => await _cmsService.ReplyGuestbookAsync(gbCode, reContent);

    [Permission("cms:guestbook:delete")]
    [HttpPost("guestbook/delete")]
    // 方法 GuestbookDelete
    // 方法：GuestbookDelete

    public async Task<ApiResult> GuestbookDelete([FromBody] DeleteGuestbookRequest request)
        => await _cmsService.DeleteGuestbookAsync(request.GbCode);

    // --- Tags ---

    [AllowAnonymous]
    [HttpGet("tag/list")]
    // 方法 TagList
    // 方法：TagList

    public async Task<ApiResult<List<TagDto>>> TagList()
        => ApiResult<List<TagDto>>.Ok(await _cmsService.GetAllTagsAsync());

    [Permission("cms:tag:save")]
    [HttpPost("tag/save")]
    // 方法 TagSave
    // 方法：TagSave

    public async Task<ApiResult> TagSave([FromQuery] string tagName)
        => await _cmsService.SaveTagAsync(tagName);

    [Permission("cms:tag:delete")]
    [HttpPost("tag/delete")]
    // 方法 TagDelete
    // 方法：TagDelete

    public async Task<ApiResult> TagDelete([FromBody] DeleteTagRequest request)
        => await _cmsService.DeleteTagAsync(request.TagName);

    // --- Visit Log ---

    [HttpPost("visit-log/add")]
    // 方法 VisitLogAdd
    // 方法：VisitLogAdd

    public async Task<ApiResult> VisitLogAdd([FromBody] VisitLog log)
        => await _cmsService.AddVisitLogAsync(log);

    [Permission("cms:visit-log:list")]
    [HttpPost("visit-log/list")]
    // 方法 VisitLogList
    // 方法：VisitLogList

    public async Task<ApiResult<PageResult<VisitLogDto>>> VisitLogList([FromBody] PageRequest<VisitLog> request)
        => ApiResult<PageResult<VisitLogDto>>.Ok(await _cmsService.FindVisitLogPageAsync(request));

    [AllowAnonymous]
    [HttpGet("stats/today-visits")]
    // 方法 TodayVisits
    // 方法：TodayVisits

    public async Task<ApiResult<long>> TodayVisits()
        => ApiResult<long>.Ok(await _cmsService.GetTodayVisitCountAsync());

    // --- Report ---

    [AllowAnonymous]
    [HttpPost("report/save")]
    // 方法 ReportSave
    // 方法：ReportSave

    public async Task<ApiResult> ReportSave([FromBody] ReportSaveDto dto)
        => await _cmsService.SaveReportAsync(dto);

    [Permission("cms:report:list")]
    [HttpPost("report/list")]
    // 方法 ReportList
    // 方法：ReportList

    public async Task<ApiResult<PageResult<ReportDto>>> ReportList([FromBody] PageRequest<Report> request)
        => ApiResult<PageResult<ReportDto>>.Ok(await _cmsService.FindReportPageAsync(request));

    [Permission("cms:report:deal")]
    [HttpPost("report/deal")]
    // 方法 ReportDeal
    // 方法：ReportDeal

    public async Task<ApiResult> ReportDeal([FromQuery] string reportCode, [FromQuery] string dealResult)
        => await _cmsService.DealReportAsync(reportCode, dealResult);

    [Permission("cms:report:delete")]
    [HttpPost("report/delete")]
    // 方法 ReportDelete
    // 方法：ReportDelete

    public async Task<ApiResult> ReportDelete([FromBody] DeleteReportRequest request)
        => await _cmsService.DeleteReportAsync(request.ReportCode);
}

// 定义class DeleteCommentRequest
// 定义类：DeleteCommentRequest

public class DeleteCommentRequest { public string CommentCode { get; set; } = string.Empty; }
// 定义class DeleteGuestbookRequest
// 定义类：DeleteGuestbookRequest

public class DeleteGuestbookRequest { public string GbCode { get; set; } = string.Empty; }
// 定义class DeleteTagRequest
// 定义类：DeleteTagRequest

public class DeleteTagRequest { public string TagName { get; set; } = string.Empty; }
// 定义class DeleteReportRequest
// 定义类：DeleteReportRequest

public class DeleteReportRequest { public string ReportCode { get; set; } = string.Empty; }
