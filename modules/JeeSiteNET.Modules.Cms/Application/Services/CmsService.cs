    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Cms.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Application.DTOs
using JeeSiteNET.Modules.Cms.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
using JeeSiteNET.Modules.Cms.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Cms.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Interfaces
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.Cms.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Application.Services
namespace JeeSiteNET.Modules.Cms.Application.Services;

// 定义class CmsService
// 定义类：CmsService
public class CmsService
{
    // 字段 _commentRepository
    // 字段：_commentRepository
    private readonly ICommentRepository _commentRepository;
    // 字段 _guestbookRepository
    // 字段：_guestbookRepository
    private readonly IGuestbookRepository _guestbookRepository;
    // 字段 _tagRepository
    // 字段：_tagRepository
    private readonly ITagRepository _tagRepository;
    // 字段 _visitLogRepository
    // 字段：_visitLogRepository
    private readonly IVisitLogRepository _visitLogRepository;
    // 字段 _articleTagRepository
    // 字段：_articleTagRepository
    private readonly IArticleTagRepository _articleTagRepository;
    // 字段 _reportRepository
    // 字段：_reportRepository
    private readonly IReportRepository _reportRepository;

    // 构造函数 CmsService
    // 构造函数：CmsService
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

    // 方法 FindCommentPageAsync
    // 方法：FindCommentPageAsync
    public async Task<PageResult<CommentDto>> FindCommentPageAsync(PageRequest<Comment> request)
    {
        var result = await _commentRepository.FindPageAsync(request);
        // return 返回结果
        return new PageResult<CommentDto>
        {
            // 数据库操作：投影选择
            List = result.List.Select(CommentDto.FromEntity).ToList(),
            Total = result.Total, PageNo = result.PageNo, PageSize = result.PageSize
        };
    }

    // 方法 SaveCommentAsync
    // 方法：SaveCommentAsync
    public async Task<ApiResult> SaveCommentAsync(CommentSaveDto dto)
    {
        // 声明并初始化变量：now
        var now = DateTime.Now;
        Comment? entity;
        // if 条件判断
        if (!string.IsNullOrEmpty(dto.CommentCode))
        {
            // 缓存：获取值
            entity = await _commentRepository.GetAsync(dto.CommentCode);
            // if 条件判断
            if (entity == null) return ApiResult.NotFound("评论不存在");
            entity.Content = dto.Content;
            entity.UpdateDate = now;
            // await 异步等待
            await _commentRepository.UpdateAsync(entity);
        }
        // else 否则分支
        else
        {
            // 创建 Comment实例并赋给 entity
            entity = new Comment
            {
                // 调用 ToString
                CommentCode = Guid.NewGuid().ToString("N")[..20],
                CategoryCode = dto.CategoryCode, ArticleCode = dto.ArticleCode,
                ParentCode = dto.ParentCode, ArticleTitle = dto.ArticleTitle,
                Content = dto.Content, Name = dto.Name,
                CreateDate = now, UpdateDate = now, Status = "0"
            };
            // await 异步等待
            await _commentRepository.AddAsync(entity);
        }
        // return 返回结果
        return ApiResult.Ok(CommentDto.FromEntity(entity));
    }

    // 方法 AuditCommentAsync
    // 方法：AuditCommentAsync
    public async Task<ApiResult> AuditCommentAsync(string commentCode, string status, string? auditComment)
    {
        // 缓存：获取值
        var entity = await _commentRepository.GetAsync(commentCode);
        // if 条件判断
        if (entity == null) return ApiResult.NotFound("评论不存在");
        entity.Status = status;
        entity.AuditDate = DateTime.Now;
        entity.AuditComment = auditComment;
        // await 异步等待
        await _commentRepository.UpdateAsync(entity);
        // return 返回结果
        return ApiResult.Ok();
    }

    // 方法 DeleteCommentAsync
    // 方法：DeleteCommentAsync
    public async Task<ApiResult> DeleteCommentAsync(string commentCode)
    {
        // 缓存：获取值
        var entity = await _commentRepository.GetAsync(commentCode);
        // if 条件判断
        if (entity == null) return ApiResult.NotFound("评论不存在");
        // await 异步等待
        await _commentRepository.DeleteAsync(entity);
        // return 返回结果
        return ApiResult.Ok();
    }

    // --- Guestbook ---

    // 方法 FindGuestbookPageAsync
    // 方法：FindGuestbookPageAsync
    public async Task<PageResult<GuestbookDto>> FindGuestbookPageAsync(PageRequest<Guestbook> request)
    {
        var result = await _guestbookRepository.FindPageAsync(request);
        // return 返回结果
        return new PageResult<GuestbookDto>
        {
            // 数据库操作：投影选择
            List = result.List.Select(GuestbookDto.FromEntity).ToList(),
            Total = result.Total, PageNo = result.PageNo, PageSize = result.PageSize
        };
    }

    // 方法 SaveGuestbookAsync
    // 方法：SaveGuestbookAsync
    public async Task<ApiResult> SaveGuestbookAsync(GuestbookSaveDto dto)
    {
        // 创建 Guestbook实例并赋给 entity
        var entity = new Guestbook
        {
            // 调用 ToString
            GbCode = Guid.NewGuid().ToString("N")[..20],
            GbType = dto.GbType, Content = dto.Content, Name = dto.Name,
            Email = dto.Email, Phone = dto.Phone, WorkUnit = dto.WorkUnit,
            Ip = string.Empty, CreateDate = DateTime.Now, UpdateDate = DateTime.Now, Status = "0"
        };
        // await 异步等待
        await _guestbookRepository.AddAsync(entity);
        // return 返回结果
        return ApiResult.Ok(GuestbookDto.FromEntity(entity));
    }

    // 方法 ReplyGuestbookAsync
    // 方法：ReplyGuestbookAsync
    public async Task<ApiResult> ReplyGuestbookAsync(string gbCode, string reContent)
    {
        // 缓存：获取值
        var entity = await _guestbookRepository.GetAsync(gbCode);
        // if 条件判断
        if (entity == null) return ApiResult.NotFound("留言不存在");
        entity.ReContent = reContent;
        entity.ReDate = DateTime.Now;
        entity.Status = "2";
        // await 异步等待
        await _guestbookRepository.UpdateAsync(entity);
        // return 返回结果
        return ApiResult.Ok();
    }

    // 方法 DeleteGuestbookAsync
    // 方法：DeleteGuestbookAsync
    public async Task<ApiResult> DeleteGuestbookAsync(string gbCode)
    {
        // 缓存：获取值
        var entity = await _guestbookRepository.GetAsync(gbCode);
        // if 条件判断
        if (entity == null) return ApiResult.NotFound("留言不存在");
        // await 异步等待
        await _guestbookRepository.DeleteAsync(entity);
        // return 返回结果
        return ApiResult.Ok();
    }

    // --- Tags ---

    // 方法 GetAllTagsAsync
    // 方法：GetAllTagsAsync
    public async Task<List<TagDto>> GetAllTagsAsync()
    {
        var list = await _tagRepository.FindListAsync();
        // return 返回结果
        return list.Select(TagDto.FromEntity).ToList();
    }

    // 方法 SaveTagAsync
    // 方法：SaveTagAsync
    public async Task<ApiResult> SaveTagAsync(string tagName)
    {
        // 缓存：获取值
        var existing = await _tagRepository.GetAsync(tagName);
        // if 条件判断
        if (existing != null) return ApiResult.Ok();
        // await 异步等待
        await _tagRepository.AddAsync(new Tag { TagName = tagName, ClickNum = 0 });
        // return 返回结果
        return ApiResult.Ok();
    }

    // 方法 DeleteTagAsync
    // 方法：DeleteTagAsync
    public async Task<ApiResult> DeleteTagAsync(string tagName)
    {
        // 缓存：获取值
        var entity = await _tagRepository.GetAsync(tagName);
        // if 条件判断
        if (entity == null) return ApiResult.NotFound("标签不存在");
        // await 异步等待
        await _tagRepository.DeleteAsync(entity);
        // return 返回结果
        return ApiResult.Ok();
    }

    // --- Visit Log ---

    // 方法 AddVisitLogAsync
    // 方法：AddVisitLogAsync
    public async Task<ApiResult> AddVisitLogAsync(VisitLog log)
    {
        // 调用 ToString
        log.VisitId = Guid.NewGuid().ToString("N")[..20];
        log.VisitTime = DateTime.Now;
        // 调用 ToString
        log.VisitDate = DateTime.Now.ToString("yyyyMMdd");
        // await 异步等待
        await _visitLogRepository.AddAsync(log);
        // return 返回结果
        return ApiResult.Ok();
    }

    // 方法 FindVisitLogPageAsync
    // 方法：FindVisitLogPageAsync
    public async Task<PageResult<VisitLogDto>> FindVisitLogPageAsync(PageRequest<VisitLog> request)
    {
        var result = await _visitLogRepository.FindPageAsync(request);
        // return 返回结果
        return new PageResult<VisitLogDto>
        {
            // 数据库操作：投影选择
            List = result.List.Select(VisitLogDto.FromEntity).ToList(),
            Total = result.Total, PageNo = result.PageNo, PageSize = result.PageSize
        };
    }

    // --- Report ---

    // 方法 FindReportPageAsync
    // 方法：FindReportPageAsync
    public async Task<PageResult<ReportDto>> FindReportPageAsync(PageRequest<Report> request)
    {
        var result = await _reportRepository.FindPageAsync(request);
        // return 返回结果
        return new PageResult<ReportDto>
        {
            // 数据库操作：投影选择
            List = result.List.Select(ReportDto.FromEntity).ToList(),
            Total = result.Total, PageNo = result.PageNo, PageSize = result.PageSize
        };
    }

    // 方法 SaveReportAsync
    // 方法：SaveReportAsync
    public async Task<ApiResult> SaveReportAsync(ReportSaveDto dto)
    {
        // 创建 Report实例并赋给 entity
        var entity = new Report
        {
            // 调用 ToString
            ReportCode = Guid.NewGuid().ToString("N")[..20],
            ArticleCode = dto.ArticleCode, ArticleTitle = dto.ArticleTitle,
            ReportType = dto.ReportType, Content = dto.Content,
            Status = "0", CreateDate = DateTime.Now
        };
        // await 异步等待
        await _reportRepository.AddAsync(entity);
        // return 返回结果
        return ApiResult.Ok();
    }

    // 方法 DealReportAsync
    // 方法：DealReportAsync
    public async Task<ApiResult> DealReportAsync(string reportCode, string dealResult)
    {
        // 缓存：获取值
        var entity = await _reportRepository.GetAsync(reportCode);
        // if 条件判断
        if (entity == null) return ApiResult.NotFound("举报记录不存在");
        entity.Status = "1";
        entity.DealResult = dealResult;
        entity.DealDate = DateTime.Now;
        // await 异步等待
        await _reportRepository.UpdateAsync(entity);
        // return 返回结果
        return ApiResult.Ok();
    }

    // 方法 DeleteReportAsync
    // 方法：DeleteReportAsync
    public async Task<ApiResult> DeleteReportAsync(string reportCode)
    {
        // 缓存：获取值
        var entity = await _reportRepository.GetAsync(reportCode);
        // if 条件判断
        if (entity == null) return ApiResult.NotFound("举报记录不存在");
        // await 异步等待
        await _reportRepository.DeleteAsync(entity);
        // return 返回结果
        return ApiResult.Ok();
    }

    // --- Helpers ---

    // 方法 GetTodayVisitCountAsync
    // 方法：GetTodayVisitCountAsync
    public async Task<long> GetTodayVisitCountAsync()
    {
        // 调用 ToString
        var today = DateTime.Now.ToString("yyyyMMdd");
        // return 返回结果
        return await _visitLogRepository.Query().CountAsync(e => e.VisitDate == today);
    }
}
