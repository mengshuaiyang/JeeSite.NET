using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>消息服务，负责站内消息发送、收件箱/发件箱、推送消息管理和消息模板维护。</summary>
public class MsgService
{
    private readonly IMsgInnerRepository _msgInnerRepository;
    private readonly IMsgPushRepository _msgPushRepository;
    private readonly IMsgPushedRepository _msgPushedRepository;
    private readonly IMsgTemplateRepository _msgTemplateRepository;
    private readonly JeeSiteDbContext _db;
    private readonly INotificationService _notification;

    /// <summary>依赖注入构造函数。</summary>
    public MsgService(
        IMsgInnerRepository msgInnerRepository,
        IMsgPushRepository msgPushRepository,
        IMsgPushedRepository msgPushedRepository,
        IMsgTemplateRepository msgTemplateRepository,
        JeeSiteDbContext db,
        INotificationService notification)
    {
        _msgInnerRepository = msgInnerRepository;
        _msgPushRepository = msgPushRepository;
        _msgPushedRepository = msgPushedRepository;
        _msgTemplateRepository = msgTemplateRepository;
        _db = db;
        _notification = notification;
    }

    /// <summary>发送站内消息：写入消息主表 + 按接收人拆分多条记录，并触发实时通知。</summary>
    /// <param name="dto">消息发送信息（含逗号分隔的接收人编码列表）。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> SendAsync(MsgInnerSaveDto dto)
    {
        var now = DateTime.Now;
        var msgId = IdGenerator.NewId();
        // 接收人编码按逗号拆分，空值安全处理
        var receiveCodes = (dto.ReceiveCodes ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries);

        var msg = new MsgInner
        {
            Id = msgId,
            MsgTitle = dto.MsgTitle,
            ContentLevel = dto.ContentLevel ?? "1",
            ContentType = dto.ContentType,
            MsgContent = dto.MsgContent,
            ReceiveType = dto.ReceiveType ?? "user",
            ReceiveCodes = dto.ReceiveCodes,
            NotifyTypes = dto.NotifyTypes,
            IsAttac = dto.IsAttac,
            SendDate = now
        };
        await _msgInnerRepository.AddAsync(msg);

        var records = receiveCodes.Select(code => new MsgInnerRecord
        {
            Id = IdGenerator.NewId(),
            MsgInnerId = msgId,
            ReceiveUserCode = code,
            ReceiveUserName = "",
            // ReadStatus = "0" 表示未读，前端读取后会更新为 "1"
            ReadStatus = "0"
        }).ToList();

        await _msgInnerRepository.AddRecordsAsync(records);
        await _msgInnerRepository.SaveChangesAsync();

        // 异步触发 SignalR/客户端实时通知
        await _notification.SendToUsersAsync(receiveCodes, dto.MsgTitle, dto.MsgContent ?? "", "msg");

        return ApiResult.Ok();
    }

    /// <summary>获取当前用户的收件箱（分页）。</summary>
    /// <param name="userCode">当前用户编码。</param>
    /// <param name="request">分页参数。</param>
    /// <returns>收件箱分页结果。</returns>
    public async Task<PageResult<MsgInnerRecordDto>> GetInboxAsync(string userCode, PageRequest request)
    {
        // 内连接 MsgInner 获取消息元信息，按发送时间倒序
        var query = from r in _db.Set<MsgInnerRecord>()
                    join m in _db.Set<MsgInner>() on r.MsgInnerId equals m.Id
                    where r.ReceiveUserCode == userCode
                    orderby m.SendDate descending
                    select new MsgInnerRecordDto
                    {
                        Id = r.Id,
                        MsgInnerId = r.MsgInnerId,
                        ReceiveUserCode = r.ReceiveUserCode,
                        ReceiveUserName = r.ReceiveUserName,
                        ReadStatus = r.ReadStatus,
                        ReadDate = r.ReadDate,
                        IsStar = r.IsStar
                    };

        var total = await query.CountAsync();
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

        return new PageResult<MsgInnerRecordDto>
        {
            List = list,
            Total = total,
            PageNo = request.PageNo,
            PageSize = request.PageSize
        };
    }

    /// <summary>获取指定用户的已发送消息列表（分页）。</summary>
    /// <param name="userCode">发送人编码。</param>
    /// <param name="request">分页参数。</param>
    /// <returns>发件箱分页结果。</returns>
    public async Task<PageResult<MsgInnerDto>> GetSentAsync(string userCode, PageRequest request)
    {
        var query = _msgInnerRepository.Query()
            .Where(m => m.SendUserCode == userCode)
            .OrderByDescending(m => m.SendDate);

        var total = await query.CountAsync();
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

        return new PageResult<MsgInnerDto>
        {
            List = list.Select(MapToInnerDto).ToList(),
            Total = total,
            PageNo = request.PageNo,
            PageSize = request.PageSize
        };
    }

    /// <summary>获取用户未读消息总数（用于前端徽标）。</summary>
    /// <param name="userCode">用户编码。</param>
    /// <returns>未读消息数量。</returns>
    public async Task<int> GetUnreadCountAsync(string userCode)
    {
        // ReadStatus = "0" 表示未读
        return await _db.Set<MsgInnerRecord>()
            .CountAsync(r => r.ReceiveUserCode == userCode && r.ReadStatus == "0");
    }

    /// <summary>标记单条消息为已读。</summary>
    /// <param name="recordId">消息记录 ID。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> MarkReadAsync(string recordId)
    {
        var record = await _db.Set<MsgInnerRecord>().FindAsync(recordId);
        if (record == null) return ApiResult.NotFound("消息不存在");
        record.ReadStatus = "1";
        record.ReadDate = DateTime.Now;
        await _db.SaveChangesAsync();
        return ApiResult.Ok();
    }

    /// <summary>删除消息记录（软删除取决于 EF Core 配置，此处为直接删除）。</summary>
    /// <param name="id">消息记录 ID。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> DeleteAsync(string id)
    {
        var record = await _db.Set<MsgInnerRecord>().FindAsync(id);
        if (record == null) return ApiResult.NotFound("消息不存在");
        _db.Set<MsgInnerRecord>().Remove(record);
        await _db.SaveChangesAsync();
        return ApiResult.Ok();
    }

    /// <summary>获取推送消息列表（分页，按创建时间倒序）。</summary>
    /// <param name="request">分页参数。</param>
    /// <returns>推送消息分页结果。</returns>
    public async Task<PageResult<MsgPushDto>> GetPushListAsync(PageRequest request)
    {
        var query = _msgPushRepository.Query()
            .OrderByDescending(p => p.CreateDate);
        var total = await query.CountAsync();
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        return new PageResult<MsgPushDto>
        {
            List = list.Select(p => new MsgPushDto
            {
                Id = p.Id,
                MsgType = p.MsgType,
                MsgTitle = p.MsgTitle,
                MsgContent = p.MsgContent,
                BizKey = p.BizKey,
                BizType = p.BizType,
                ReceiveUserCode = p.ReceiveUserCode,
                ReceiveUserName = p.ReceiveUserName,
                SendUserCode = p.SendUserCode,
                SendUserName = p.SendUserName,
                SendDate = p.SendDate,
                PushStatus = p.PushStatus,
                ReadStatus = p.ReadStatus,
            }).ToList(),
            Total = total,
            PageNo = request.PageNo,
            PageSize = request.PageSize
        };
    }

    /// <summary>创建一条待推送消息（PushStatus = pending，由后台调度任务实际推送）。</summary>
    /// <param name="dto">推送消息信息。</param>
    /// <returns>返回新创建记录的 ID。</returns>
    public async Task<ApiResult> SendPushAsync(MsgPushSaveDto dto)
    {
        var now = DateTime.Now;
        // 状态使用 "pending" 表示待推送，后续由 Quartz/BackgroundService 处理
        var entity = new MsgPush
        {
            Id = IdGenerator.NewId(),
            MsgType = dto.MsgType,
            MsgTitle = dto.MsgTitle,
            MsgContent = dto.MsgContent,
            BizKey = dto.BizKey,
            BizType = dto.BizType,
            ReceiveCode = dto.ReceiveCode ?? "",
            ReceiveUserCode = dto.ReceiveUserCode ?? "",
            ReceiveUserName = dto.ReceiveUserName ?? "",
            SendDate = now,
            PlanPushDate = dto.PlanPushDate ?? now,
            PushStatus = "pending",
            ReadStatus = "0"
        };
        await _msgPushRepository.AddAsync(entity);
        await _msgPushRepository.SaveChangesAsync();
        return ApiResult.Ok(new { entity.Id });
    }

    /// <summary>重试推送（恢复为 pending 并递增重试次数）。</summary>
    /// <param name="id">推送记录 ID。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> RetryPushAsync(string id)
    {
        var entity = await _msgPushRepository.GetAsync(id);
        if (entity == null) return ApiResult.NotFound("推送记录不存在");

        entity.PushStatus = "pending";
        // PushNumber 初始为 null 时默认 0，每次重试累加
        entity.PushNumber = (entity.PushNumber ?? 0) + 1;
        entity.PushDate = DateTime.Now;
        await _msgPushRepository.UpdateAsync(entity);
        await _msgPushRepository.SaveChangesAsync();
        return ApiResult.Ok();
    }

    /// <summary>新增或保存消息模板。</summary>
    /// <param name="dto">模板保存信息。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> SaveTemplateAsync(MsgTemplateSaveDto dto)
    {
        var now = DateTime.Now;
        if (!string.IsNullOrEmpty(dto.Id))
        {
            var entity = await _msgTemplateRepository.GetAsync(dto.Id);
            if (entity == null) return ApiResult.NotFound("模板不存在");
            entity.ModuleCode = dto.ModuleCode;
            entity.TplKey = dto.TplKey;
            entity.TplName = dto.TplName;
            entity.TplType = dto.TplType;
            entity.TplContent = dto.TplContent;
            await _msgTemplateRepository.UpdateAsync(entity);
            await _msgTemplateRepository.SaveChangesAsync();
        }
        else
        {
            var entity = new MsgTemplate
            {
                Id = IdGenerator.NewId(),
                ModuleCode = dto.ModuleCode,
                TplKey = dto.TplKey,
                TplName = dto.TplName,
                TplType = dto.TplType,
                TplContent = dto.TplContent
            };
            await _msgTemplateRepository.AddAsync(entity);
            await _msgTemplateRepository.SaveChangesAsync();
        }
        return ApiResult.Ok();
    }

    /// <summary>按模板 Key 获取模板（供业务层渲染时调用）。</summary>
    /// <param name="tplKey">模板 Key。</param>
    /// <returns>模板 DTO，不存在时返回 null。</returns>
    public async Task<MsgTemplateDto?> GetTemplateAsync(string tplKey)
    {
        var entity = await _msgTemplateRepository.GetByKeyAsync(tplKey);
        return entity == null ? null : MapToTemplateDto(entity);
    }

    /// <summary>站内消息实体到 DTO 的转换映射。</summary>
    private static MsgInnerDto MapToInnerDto(MsgInner e) => new()
    {
        Id = e.Id,
        MsgTitle = e.MsgTitle,
        ContentLevel = e.ContentLevel,
        ContentType = e.ContentType,
        MsgContent = e.MsgContent,
        ReceiveType = e.ReceiveType,
        SendUserCode = e.SendUserCode,
        SendUserName = e.SendUserName,
        SendDate = e.SendDate,
        Status = e.Status,
        NotifyTypes = e.NotifyTypes
    };

    /// <summary>消息模板实体到 DTO 的转换映射。</summary>
    private static MsgTemplateDto MapToTemplateDto(MsgTemplate e) => new()
    {
        Id = e.Id,
        ModuleCode = e.ModuleCode,
        TplKey = e.TplKey,
        TplName = e.TplName,
        TplType = e.TplType,
        TplContent = e.TplContent,
        Status = e.Status
    };
}
