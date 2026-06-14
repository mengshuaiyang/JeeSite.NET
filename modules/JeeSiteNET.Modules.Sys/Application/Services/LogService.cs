using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>操作日志服务，负责日志记录与查询。</summary>
public class LogService
{
    private readonly ILogRepository _logRepository;

    /// <summary>依赖注入构造函数。</summary>
    public LogService(ILogRepository logRepository) => _logRepository = logRepository;

    /// <summary>根据日志 ID 获取日志详情。</summary>
    /// <param name="logId">日志 ID。</param>
    /// <returns>日志 DTO，不存在时返回 null。</returns>
    public async Task<LogDto?> GetAsync(string logId)
    {
        var entity = await _logRepository.GetAsync(logId);
        return entity == null ? null : MapToDto(entity);
    }

    /// <summary>按类型/标题/用户过滤分页查询日志。</summary>
    /// <param name="request">分页与过滤条件。</param>
    /// <returns>日志分页结果。</returns>
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

    /// <summary>写入一条操作日志（通常由审计中间件或接口拦截器调用）。</summary>
    /// <param name="entity">日志实体。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> SaveAsync(Log entity)
    {
        await _logRepository.AddAsync(entity);
        return ApiResult.Ok();
    }

    /// <summary>实体到 DTO 的转换映射。</summary>
    private static LogDto MapToDto(Log e) => new()
    {
        LogId = e.LogId, LogType = e.LogType, LogTitle = e.LogTitle,
        RequestUri = e.RequestUri, RequestMethod = e.RequestMethod,
        ExecuteTime = e.ExecuteTime, UserCode = e.UserCode, UserName = e.UserName,
        CreateByName = e.CreateByName,
        OrgCode = e.OrgCode, RemoteIp = e.RemoteIp, CreateDate = e.CreateDate
    };
}
