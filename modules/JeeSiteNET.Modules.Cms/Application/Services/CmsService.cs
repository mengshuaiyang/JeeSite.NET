using JeeSiteNET.Core;
using JeeSiteNET.Modules.Cms.Application.DTOs;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Cms.Application.Services;

public class CmsService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IGuestbookRepository _guestbookRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IVisitLogRepository _visitLogRepository;
    private readonly IArticleTagRepository _articleTagRepository;
    private readonly IReportRepository _reportRepository;

    public CmsService(
        ICommentRepository commentRepository,
        IGuestbookRepository guestbookRepository,
        ITagRepository tagRepository,
        IVisitLogRepository visitLogRepository,
        IArticleTagRepository articleTagRepository,
        IReportRepository reportRepository)
    {
        _commentRepository = commentRepository;
        _guestbookRepository = guestbookRepository;
        _tagRepository = tagRepository;
        _visitLogRepository = visitLogRepository;
        _articleTagRepository = articleTagRepository;
        _reportRepository = reportRepository;
    }

    // --- Comments ---

    public async Task<PageResult<CommentDto>> FindCommentPageAsync(PageRequest<Comment> request)
    {
        var result = await _commentRepository.FindPageAsync(request);
        return new PageResult<CommentDto>
        {
            List = result.List.Select(CommentDto.FromEntity).ToList(),
            Total = result.Total, PageNo = result.PageNo, PageSize = result.PageSize
        };
    }

    public async Task<ApiResult> SaveCommentAsync(CommentSaveDto dto)
    {
        var now = DateTime.Now;
        Comment? entity;
        if (!string.IsNullOrEmpty(dto.CommentCode))
        {
            entity = await _commentRepository.GetAsync(dto.CommentCode);
            if (entity == null) return ApiResult.NotFound("评论不存在");
            entity.Content = dto.Content;
            entity.UpdateDate = now;
            await _commentRepository.UpdateAsync(entity);
        }
        else
        {
            entity = new Comment
            {
                CommentCode = Guid.NewGuid().ToString("N")[..20],
                CategoryCode = dto.CategoryCode, ArticleCode = dto.ArticleCode,
                ParentCode = dto.ParentCode, ArticleTitle = dto.ArticleTitle,
                Content = dto.Content, Name = dto.Name,
                CreateDate = now, UpdateDate = now, Status = "0"
            };
            await _commentRepository.AddAsync(entity);
        }
        return ApiResult.Ok(CommentDto.FromEntity(entity));
    }

    public async Task<ApiResult> AuditCommentAsync(string commentCode, string status, string? auditComment)
    {
        var entity = await _commentRepository.GetAsync(commentCode);
        if (entity == null) return ApiResult.NotFound("评论不存在");
        entity.Status = status;
        entity.AuditDate = DateTime.Now;
        entity.AuditComment = auditComment;
        await _commentRepository.UpdateAsync(entity);
        return ApiResult.Ok();
    }

    public async Task<ApiResult> DeleteCommentAsync(string commentCode)
    {
        var entity = await _commentRepository.GetAsync(commentCode);
        if (entity == null) return ApiResult.NotFound("评论不存在");
        await _commentRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    // --- Guestbook ---

    public async Task<PageResult<GuestbookDto>> FindGuestbookPageAsync(PageRequest<Guestbook> request)
    {
        var result = await _guestbookRepository.FindPageAsync(request);
        return new PageResult<GuestbookDto>
        {
            List = result.List.Select(GuestbookDto.FromEntity).ToList(),
            Total = result.Total, PageNo = result.PageNo, PageSize = result.PageSize
        };
    }

    public async Task<ApiResult> SaveGuestbookAsync(GuestbookSaveDto dto)
    {
        var entity = new Guestbook
        {
            GbCode = Guid.NewGuid().ToString("N")[..20],
            GbType = dto.GbType, Content = dto.Content, Name = dto.Name,
            Email = dto.Email, Phone = dto.Phone, WorkUnit = dto.WorkUnit,
            Ip = string.Empty, CreateDate = DateTime.Now, UpdateDate = DateTime.Now, Status = "0"
        };
        await _guestbookRepository.AddAsync(entity);
        return ApiResult.Ok(GuestbookDto.FromEntity(entity));
    }

    public async Task<ApiResult> ReplyGuestbookAsync(string gbCode, string reContent)
    {
        var entity = await _guestbookRepository.GetAsync(gbCode);
        if (entity == null) return ApiResult.NotFound("留言不存在");
        entity.ReContent = reContent;
        entity.ReDate = DateTime.Now;
        entity.Status = "2";
        await _guestbookRepository.UpdateAsync(entity);
        return ApiResult.Ok();
    }

    public async Task<ApiResult> DeleteGuestbookAsync(string gbCode)
    {
        var entity = await _guestbookRepository.GetAsync(gbCode);
        if (entity == null) return ApiResult.NotFound("留言不存在");
        await _guestbookRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    // --- Tags ---

    public async Task<List<TagDto>> GetAllTagsAsync()
    {
        var list = await _tagRepository.FindListAsync();
        return list.Select(TagDto.FromEntity).ToList();
    }

    public async Task<ApiResult> SaveTagAsync(string tagName)
    {
        var existing = await _tagRepository.GetAsync(tagName);
        if (existing != null) return ApiResult.Ok();
        await _tagRepository.AddAsync(new Tag { TagName = tagName, ClickNum = 0 });
        return ApiResult.Ok();
    }

    public async Task<ApiResult> DeleteTagAsync(string tagName)
    {
        var entity = await _tagRepository.GetAsync(tagName);
        if (entity == null) return ApiResult.NotFound("标签不存在");
        await _tagRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    // --- Visit Log ---

    public async Task<ApiResult> AddVisitLogAsync(VisitLog log)
    {
        log.VisitId = Guid.NewGuid().ToString("N")[..20];
        log.VisitTime = DateTime.Now;
        log.VisitDate = DateTime.Now.ToString("yyyyMMdd");
        await _visitLogRepository.AddAsync(log);
        return ApiResult.Ok();
    }

    public async Task<PageResult<VisitLogDto>> FindVisitLogPageAsync(PageRequest<VisitLog> request)
    {
        var result = await _visitLogRepository.FindPageAsync(request);
        return new PageResult<VisitLogDto>
        {
            List = result.List.Select(VisitLogDto.FromEntity).ToList(),
            Total = result.Total, PageNo = result.PageNo, PageSize = result.PageSize
        };
    }

    // --- Report ---

    public async Task<PageResult<ReportDto>> FindReportPageAsync(PageRequest<Report> request)
    {
        var result = await _reportRepository.FindPageAsync(request);
        return new PageResult<ReportDto>
        {
            List = result.List.Select(ReportDto.FromEntity).ToList(),
            Total = result.Total, PageNo = result.PageNo, PageSize = result.PageSize
        };
    }

    public async Task<ApiResult> SaveReportAsync(ReportSaveDto dto)
    {
        var entity = new Report
        {
            ReportCode = Guid.NewGuid().ToString("N")[..20],
            ArticleCode = dto.ArticleCode, ArticleTitle = dto.ArticleTitle,
            ReportType = dto.ReportType, Content = dto.Content,
            Status = "0", CreateDate = DateTime.Now
        };
        await _reportRepository.AddAsync(entity);
        return ApiResult.Ok();
    }

    public async Task<ApiResult> DealReportAsync(string reportCode, string dealResult)
    {
        var entity = await _reportRepository.GetAsync(reportCode);
        if (entity == null) return ApiResult.NotFound("举报记录不存在");
        entity.Status = "1";
        entity.DealResult = dealResult;
        entity.DealDate = DateTime.Now;
        await _reportRepository.UpdateAsync(entity);
        return ApiResult.Ok();
    }

    public async Task<ApiResult> DeleteReportAsync(string reportCode)
    {
        var entity = await _reportRepository.GetAsync(reportCode);
        if (entity == null) return ApiResult.NotFound("举报记录不存在");
        await _reportRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    // --- Helpers ---

    public async Task<long> GetTodayVisitCountAsync()
    {
        var today = DateTime.Now.ToString("yyyyMMdd");
        return await _visitLogRepository.Query().CountAsync(e => e.VisitDate == today);
    }
}
