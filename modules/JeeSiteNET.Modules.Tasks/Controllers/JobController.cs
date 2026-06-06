using JeeSiteNET.Core;
using JeeSiteNET.Modules.Tasks.Application.DTOs;
using JeeSiteNET.Modules.Tasks.Application.Services;
using JeeSiteNET.Modules.Tasks.Domain.Entities;
using JeeSiteNET.Core.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Tasks.Controllers;

[ApiController]
[Route("api/v1/tasks/job")]
public class JobController : ControllerBase
{
    private readonly SchedulerService _schedulerService;
    public JobController(SchedulerService schedulerService) => _schedulerService = schedulerService;

    [Permission("tasks:job:list")]
    [HttpPost("list")]
    public async Task<ApiResult<PageResult<SysJobDto>>> List([FromBody] PageRequest<SysJob> request)
        => ApiResult<PageResult<SysJobDto>>.Ok(await _schedulerService.FindPageAsync(request));

    [Permission("tasks:job:list")]
    [HttpGet("get")]
    public async Task<ApiResult<SysJobDto?>> Get([FromQuery] string jobId)
    {
        var dto = await _schedulerService.GetAsync(jobId);
        return dto == null ? ApiResult<SysJobDto?>.NotFound("任务不存在") : ApiResult<SysJobDto?>.Ok(dto);
    }

    [Permission("tasks:job:edit")]
    [HttpPost("save")]
    public async Task<ApiResult> Save([FromBody] SysJobSaveDto dto) => await _schedulerService.SaveAsync(dto);

    [Permission("tasks:job:delete")]
    [HttpPost("delete")]
    public async Task<ApiResult> Delete([FromBody] DeleteJobRequest request) => await _schedulerService.DeleteAsync(request.JobId);

    [Permission("tasks:job:edit")]
    [HttpPost("start")]
    public async Task<ApiResult> Start([FromBody] JobActionRequest request) => await _schedulerService.StartJobAsync(request.JobId);

    [Permission("tasks:job:edit")]
    [HttpPost("stop")]
    public async Task<ApiResult> Stop([FromBody] JobActionRequest request) => await _schedulerService.StopJobAsync(request.JobId);

    [Permission("tasks:job:edit")]
    [HttpPost("run")]
    public async Task<ApiResult> RunOnce([FromBody] JobActionRequest request) => await _schedulerService.RunOnceAsync(request.JobId);

    [Permission("tasks:job:list")]
    [HttpGet("logs")]
    public async Task<ApiResult<List<JobLogDto>>> Logs([FromQuery] string jobId)
        => ApiResult<List<JobLogDto>>.Ok(await _schedulerService.GetLogsAsync(jobId));
}

public class DeleteJobRequest { public string JobId { get; set; } = string.Empty; }
public class JobActionRequest { public string JobId { get; set; } = string.Empty; }
