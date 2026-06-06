using JeeSiteNET.Core;
using JeeSiteNET.Modules.Tasks.Application.DTOs;
using JeeSiteNET.Modules.Tasks.Domain.Entities;
using JeeSiteNET.Modules.Tasks.Domain.Interfaces;
using JeeSiteNET.Modules.Tasks.Jobs;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace JeeSiteNET.Modules.Tasks.Application.Services;

public class SchedulerService
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly ISysJobRepository _sysJobRepository;
    private readonly IJobLogRepository _jobLogRepository;

    public SchedulerService(ISchedulerFactory schedulerFactory, ISysJobRepository sysJobRepository, IJobLogRepository jobLogRepository)
    {
        _schedulerFactory = schedulerFactory;
        _sysJobRepository = sysJobRepository;
        _jobLogRepository = jobLogRepository;
    }

    public async Task<PageResult<SysJobDto>> FindPageAsync(PageRequest<SysJob> request)
    {
        var query = _sysJobRepository.Query()
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.JobName), j => j.JobName.Contains(request.Entity!.JobName!))
            .OrderBy(j => j.JobId);
        var total = await query.CountAsync();
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        return new PageResult<SysJobDto> { List = list.Select(SysJobDto.FromEntity).ToList(), Total = total };
    }

    public async Task<SysJobDto?> GetAsync(string jobId)
    {
        var entity = await _sysJobRepository.GetAsync(jobId);
        return entity == null ? null : SysJobDto.FromEntity(entity);
    }

    public async Task<ApiResult> SaveAsync(SysJobSaveDto dto)
    {
        var now = DateTime.Now;
        SysJob? entity;
        if (!string.IsNullOrEmpty(dto.JobId) && await _sysJobRepository.GetAsync(dto.JobId) != null)
        {
            entity = await _sysJobRepository.GetAsync(dto.JobId);
            if (entity == null) return ApiResult.NotFound("任务不存在");
            entity.JobName = dto.JobName; entity.JobGroup = dto.JobGroup; entity.CronExpression = dto.CronExpression;
            entity.AssemblyName = dto.AssemblyName; entity.ClassName = dto.ClassName; entity.Description = dto.Description;
            entity.UpdateDate = now;
            await _sysJobRepository.UpdateAsync(entity);
        }
        else
        {
            entity = new SysJob
            {
                JobId = dto.JobName, JobName = dto.JobName, JobGroup = dto.JobGroup ?? "DEFAULT",
                CronExpression = dto.CronExpression, AssemblyName = dto.AssemblyName, ClassName = dto.ClassName,
                Description = dto.Description, CreateDate = now, UpdateDate = now
            };
            await _sysJobRepository.AddAsync(entity);
        }
        return ApiResult.Ok(SysJobDto.FromEntity(entity));
    }

    public async Task<ApiResult> DeleteAsync(string jobId)
    {
        var entity = await _sysJobRepository.GetAsync(jobId);
        if (entity == null) return ApiResult.NotFound("任务不存在");
        await StopJobAsync(jobId);
        await _sysJobRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    public async Task<ApiResult> StartJobAsync(string jobId)
    {
        var entity = await _sysJobRepository.GetAsync(jobId);
        if (entity == null) return ApiResult.NotFound("任务不存在");

        var scheduler = await _schedulerFactory.GetScheduler();
        var jobKey = new JobKey(jobId, entity.JobGroup ?? "DEFAULT");

        if (await scheduler.CheckExists(jobKey))
            return ApiResult.Fail(400, "任务已在运行中");

        var jobType = !string.IsNullOrEmpty(entity.ClassName)
            ? Type.GetType($"{entity.ClassName}, {entity.AssemblyName}") ?? typeof(SampleJob)
            : typeof(SampleJob);

        var job = JobBuilder.Create(jobType)
            .WithIdentity(jobKey)
            .WithDescription(entity.Description)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"{jobId}.trigger", entity.JobGroup ?? "DEFAULT")
            .WithCronSchedule(entity.CronExpression)
            .Build();

        await scheduler.ScheduleJob(job, trigger);

        entity.RunStatus = "1";
        entity.UpdateDate = DateTime.Now;
        await _sysJobRepository.UpdateAsync(entity);
        return ApiResult.Ok();
    }

    public async Task<ApiResult> StopJobAsync(string jobId)
    {
        var entity = await _sysJobRepository.GetAsync(jobId);
        if (entity == null) return ApiResult.NotFound("任务不存在");

        var scheduler = await _schedulerFactory.GetScheduler();
        var jobKey = new JobKey(jobId, entity.JobGroup ?? "DEFAULT");

        if (await scheduler.CheckExists(jobKey))
            await scheduler.DeleteJob(jobKey);

        entity.RunStatus = "0";
        entity.UpdateDate = DateTime.Now;
        await _sysJobRepository.UpdateAsync(entity);
        return ApiResult.Ok();
    }

    public async Task<ApiResult> RunOnceAsync(string jobId)
    {
        var entity = await _sysJobRepository.GetAsync(jobId);
        if (entity == null) return ApiResult.NotFound("任务不存在");

        var scheduler = await _schedulerFactory.GetScheduler();
        var jobKey = new JobKey(jobId, entity.JobGroup ?? "DEFAULT");
        await scheduler.TriggerJob(jobKey);
        return ApiResult.Ok();
    }

    public async Task<List<JobLogDto>> GetLogsAsync(string jobId)
    {
        var logs = await _jobLogRepository.FindByJobIdAsync(jobId);
        return logs.Select(JobLogDto.FromEntity).ToList();
    }

    public async Task InitDefaultJobsAsync()
    {
        if ((await _sysJobRepository.FindListAsync()).Count > 0) return;

        var now = DateTime.Now;
        var jobs = new List<SysJob>
        {
            new() { JobId = "sys_clean_log", JobName = "日志清理", JobGroup = "SYSTEM", CronExpression = "0 0 3 * * ?", AssemblyName = "JeeSiteNET.Modules.Tasks", ClassName = "JeeSiteNET.Modules.Tasks.Jobs.SampleJob", Description = "每天凌晨3点清理过期日志", RunStatus = "0", CreateDate = now, UpdateDate = now },
            new() { JobId = "sys_heartbeat", JobName = "系统心跳", JobGroup = "SYSTEM", CronExpression = "0 */5 * * * ?", AssemblyName = "JeeSiteNET.Modules.Tasks", ClassName = "JeeSiteNET.Modules.Tasks.Jobs.SampleJob", Description = "每5分钟心跳检测", RunStatus = "0", CreateDate = now, UpdateDate = now }
        };
        foreach (var job in jobs)
            await _sysJobRepository.AddAsync(job);
    }
}
