    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Tasks.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Tasks.Application.DTOs
using JeeSiteNET.Modules.Tasks.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Tasks.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Tasks.Application.Services
using JeeSiteNET.Modules.Tasks.Application.Services;
    // 引入 JeeSiteNET.Modules.Tasks.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Tasks.Domain.Entities
using JeeSiteNET.Modules.Tasks.Domain.Entities;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 Microsoft.AspNetCore.Authorization 命名空间
// 引入命名空间：Microsoft.AspNetCore.Authorization
using Microsoft.AspNetCore.Authorization;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Tasks.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Tasks.Controllers
namespace JeeSiteNET.Modules.Tasks.Controllers;

[ApiController]
[Route("api/v1/tasks/job")]
// 定义class JobController
// 定义类：JobController

public class JobController : ControllerBase
{
    // 字段 _schedulerService
    // 字段：_schedulerService

    private readonly SchedulerService _schedulerService;
    // 构造函数 JobController
    // 构造函数：JobController

    public JobController(SchedulerService schedulerService) => _schedulerService = schedulerService;

    [Permission("tasks:job:list")]
    [HttpPost("list")]
    // 方法：List

    public async Task<ApiResult<PageResult<SysJobDto>>> List([FromBody] PageRequest<SysJob> request)
        => ApiResult<PageResult<SysJobDto>>.Ok(await _schedulerService.FindPageAsync(request));

    [Permission("tasks:job:list")]
    [HttpGet("get")]
    // 方法 Get
    // 方法：Get

    public async Task<ApiResult<SysJobDto?>> Get([FromQuery] string jobId)
    {
        // 缓存：获取值
        var dto = await _schedulerService.GetAsync(jobId);
        // return 返回结果
        return dto == null ? ApiResult<SysJobDto?>.NotFound("任务不存在") : ApiResult<SysJobDto?>.Ok(dto);
    }

    [Permission("tasks:job:edit")]
    [HttpPost("save")]
    // 方法：Save

    public async Task<ApiResult> Save([FromBody] SysJobSaveDto dto) => await _schedulerService.SaveAsync(dto);

    [Permission("tasks:job:delete")]
    [HttpPost("delete")]
    // 方法：Delete

    public async Task<ApiResult> Delete([FromBody] DeleteJobRequest request) => await _schedulerService.DeleteAsync(request.JobId);

    [Permission("tasks:job:edit")]
    [HttpPost("start")]
    // 方法：Start

    public async Task<ApiResult> Start([FromBody] JobActionRequest request) => await _schedulerService.StartJobAsync(request.JobId);

    [Permission("tasks:job:edit")]
    [HttpPost("stop")]
    // 方法：Stop

    public async Task<ApiResult> Stop([FromBody] JobActionRequest request) => await _schedulerService.StopJobAsync(request.JobId);

    [Permission("tasks:job:edit")]
    [HttpPost("run")]
    // 方法：RunOnce

    public async Task<ApiResult> RunOnce([FromBody] JobActionRequest request) => await _schedulerService.RunOnceAsync(request.JobId);

    [Permission("tasks:job:list")]
    [HttpGet("logs")]
    // 方法 Logs
    // 方法：Logs

    public async Task<ApiResult<List<JobLogDto>>> Logs([FromQuery] string jobId)
        => ApiResult<List<JobLogDto>>.Ok(await _schedulerService.GetLogsAsync(jobId));
}

// 定义class DeleteJobRequest
// 定义类：DeleteJobRequest

public class DeleteJobRequest { public string JobId { get; set; } = string.Empty; }
// 定义class JobActionRequest
// 定义类：JobActionRequest

public class JobActionRequest { public string JobId { get; set; } = string.Empty; }
