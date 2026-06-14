using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

/// <summary>系统日志查询接口控制器，提供操作日志分页查询与单条详情获取接口。</summary>
[ApiController]
[Route("api/v1/sys/log")]
public class LogController : ControllerBase

{
    private readonly LogService _logService;

    public LogController(LogService logService) => _logService = logService;

    /// <summary>HTTP POST - 分页查询列表，支持筛选与排序条件。</summary>
    [Permission("sys:log:list")]
    [HttpPost("list")]
    public async Task<ApiResult<PageResult<LogDto>>> List([FromBody] PageRequest<Log> request)
    {
        return ApiResult<PageResult<LogDto>>.Ok(await _logService.FindPageAsync(request));
    }

    /// <summary>HTTP GET - 根据主键获取单条详情。</summary>
    [Permission("sys:log:list")]
    [HttpGet("get")]
    public async Task<ApiResult<LogDto?>> Get([FromQuery] string logId)
    {
        var entity = await _logService.GetAsync(logId);

        return entity == null ? ApiResult<LogDto?>.NotFound("日志不存在") : ApiResult<LogDto?>.Ok(entity);
    }
}
