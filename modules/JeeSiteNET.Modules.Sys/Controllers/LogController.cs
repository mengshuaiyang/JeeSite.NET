using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/log")]
public class LogController : ControllerBase
{
    private readonly LogService _logService;
    public LogController(LogService logService) => _logService = logService;

    [Permission("sys:log:list")]
    [HttpPost("list")]
    public async Task<ApiResult<PageResult<LogDto>>> List([FromBody] PageRequest<Log> request)
    {
        return ApiResult<PageResult<LogDto>>.Ok(await _logService.FindPageAsync(request));
    }

    [Permission("sys:log:list")]
    [HttpGet("get")]
    public async Task<ApiResult<LogDto?>> Get([FromQuery] string logId)
    {
        var entity = await _logService.GetAsync(logId);
        return entity == null ? ApiResult<LogDto?>.NotFound("日志不存在") : ApiResult<LogDto?>.Ok(entity);
    }
}
