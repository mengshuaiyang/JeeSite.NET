using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class MsgService
{
    private readonly IMsgInnerRepository _msgInnerRepository;
    private readonly IMsgPushRepository _msgPushRepository;
    private readonly IMsgPushedRepository _msgPushedRepository;
    private readonly IMsgTemplateRepository _msgTemplateRepository;
    private readonly DbContext _db;

    public MsgService(
        IMsgInnerRepository msgInnerRepository,
        IMsgPushRepository msgPushRepository,
        IMsgPushedRepository msgPushedRepository,
        IMsgTemplateRepository msgTemplateRepository,
        DbContext db)
    {
        _msgInnerRepository = msgInnerRepository;
        _msgPushRepository = msgPushRepository;
        _msgPushedRepository = msgPushedRepository;
        _msgTemplateRepository = msgTemplateRepository;
        _db = db;
    }

    public async Task<ApiResult> SendAsync(MsgInnerSaveDto dto)
    {
        var now = DateTime.Now;
        var msgId = IdGenerator.NewId();
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
            ReadStatus = "0"
        }).ToList();

        await _msgInnerRepository.AddRecordsAsync(records);
        await _msgInnerRepository.SaveChangesAsync();

        return ApiResult.Ok();
    }

    public async Task<PageResult<MsgInnerRecordDto>> GetInboxAsync(string userCode, PageRequest request)
    {
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

    public async Task<int> GetUnreadCountAsync(string userCode)
    {
        return await _db.Set<MsgInnerRecord>()
            .CountAsync(r => r.ReceiveUserCode == userCode && r.ReadStatus == "0");
    }

    public async Task<ApiResult> MarkReadAsync(string recordId)
    {
        var record = await _db.Set<MsgInnerRecord>().FindAsync(recordId);
        if (record == null) return ApiResult.NotFound("消息不存在");
        record.ReadStatus = "1";
        record.ReadDate = DateTime.Now;
        await _db.SaveChangesAsync();
        return ApiResult.Ok();
    }

    public async Task<ApiResult> DeleteAsync(string id)
    {
        var record = await _db.Set<MsgInnerRecord>().FindAsync(id);
        if (record == null) return ApiResult.NotFound("消息不存在");
        _db.Set<MsgInnerRecord>().Remove(record);
        await _db.SaveChangesAsync();
        return ApiResult.Ok();
    }

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

    public async Task<MsgTemplateDto?> GetTemplateAsync(string tplKey)
    {
        var entity = await _msgTemplateRepository.GetByKeyAsync(tplKey);
        return entity == null ? null : MapToTemplateDto(entity);
    }

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
