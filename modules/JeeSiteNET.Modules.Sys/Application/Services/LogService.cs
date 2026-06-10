using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class LogService
{
    private readonly ILogRepository _logRepository;
    public LogService(ILogRepository logRepository) => _logRepository = logRepository;

    public async Task<LogDto?> GetAsync(string logId)
    {
        var entity = await _logRepository.GetAsync(logId);
        return entity == null ? null : MapToDto(entity);
    }

    public async Task<PageResult<LogDto>> FindPageAsync(PageRequest<Log> request)
    {
        var query = _logRepository.Query()
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.LogType), l => l.LogType == request.Entity!.LogType)
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.LogTitle), l => l.LogTitle!.Contains(request.Entity!.LogTitle!))
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.UserCode), l => l.UserCode == request.Entity!.UserCode)
            .OrderByDescending(l => l.CreateDate);
        var total = await query.CountAsync();
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        return new PageResult<LogDto> { List = list.Select(MapToDto).ToList(), Total = total, PageNo = request.PageNo, PageSize = request.PageSize };
    }

    public async Task<ApiResult> SaveAsync(Log entity)
    {
        await _logRepository.AddAsync(entity);
        return ApiResult.Ok();
    }

    private static LogDto MapToDto(Log e) => new()
    {
        LogId = e.LogId, LogType = e.LogType, LogTitle = e.LogTitle,
        RequestUri = e.RequestUri, RequestMethod = e.RequestMethod,
        ExecuteTime = e.ExecuteTime, UserCode = e.UserCode, UserName = e.UserName,
        CreateByName = e.CreateByName,
        OrgCode = e.OrgCode, RemoteIp = e.RemoteIp, CreateDate = e.CreateDate
    };
}
