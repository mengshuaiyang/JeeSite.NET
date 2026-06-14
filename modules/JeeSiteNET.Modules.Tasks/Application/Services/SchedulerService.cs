    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Tasks.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Tasks.Application.DTOs
using JeeSiteNET.Modules.Tasks.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Tasks.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Tasks.Domain.Entities
using JeeSiteNET.Modules.Tasks.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Tasks.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Tasks.Domain.Interfaces
using JeeSiteNET.Modules.Tasks.Domain.Interfaces;
    // 引入 JeeSiteNET.Modules.Tasks.Jobs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Tasks.Jobs
using JeeSiteNET.Modules.Tasks.Jobs;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;
    // 引入 Quartz 命名空间
// 引入命名空间：Quartz
using Quartz;

// 定义 JeeSiteNET.Modules.Tasks.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Tasks.Application.Services
namespace JeeSiteNET.Modules.Tasks.Application.Services;

// 定义class SchedulerService
// 定义类：SchedulerService
public class SchedulerService
{
    // 字段 _schedulerFactory
    // 字段：_schedulerFactory
    private readonly ISchedulerFactory _schedulerFactory;
    // 字段 _sysJobRepository
    // 字段：_sysJobRepository
    private readonly ISysJobRepository _sysJobRepository;
    // 字段 _jobLogRepository
    // 字段：_jobLogRepository
    private readonly IJobLogRepository _jobLogRepository;

    // 方法 SchedulerService
    // 构造函数：SchedulerService
    public SchedulerService(ISchedulerFactory schedulerFactory, ISysJobRepository sysJobRepository, IJobLogRepository jobLogRepository)
    {
        _schedulerFactory = schedulerFactory;
        _sysJobRepository = sysJobRepository;
        _jobLogRepository = jobLogRepository;
    }

    // 方法 FindPageAsync
    // 方法：FindPageAsync
    public async Task<PageResult<SysJobDto>> FindPageAsync(PageRequest<SysJob> request)
    {
        // 调用 Query
        var query = _sysJobRepository.Query()
            // 集合操作：检查是否包含
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.JobName), j => j.JobName.Contains(request.Entity!.JobName!))
            // 数据库操作：升序排序
            .OrderBy(j => j.JobId);
        // 数据库操作：异步统计数量
        var total = await query.CountAsync();
        // 数据库操作：异步查询为列表
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        // return 返回结果
        return new PageResult<SysJobDto> { List = list.Select(SysJobDto.FromEntity).ToList(), Total = total };
    }

    // 方法 GetAsync
    // 方法：GetAsync
    public async Task<SysJobDto?> GetAsync(string jobId)
    {
        // 缓存：获取值
        var entity = await _sysJobRepository.GetAsync(jobId);
        // return 返回结果
        return entity == null ? null : SysJobDto.FromEntity(entity);
    }

    // 方法 SaveAsync
    // 方法：SaveAsync
    public async Task<ApiResult> SaveAsync(SysJobSaveDto dto)
    {
        // 声明并初始化变量：now
        var now = DateTime.Now;
        SysJob? entity;
        // if 条件判断
        if (!string.IsNullOrEmpty(dto.JobId) && await _sysJobRepository.GetAsync(dto.JobId) != null)
        {
            // 缓存：获取值
            entity = await _sysJobRepository.GetAsync(dto.JobId);
            // if 条件判断
            if (entity == null) return ApiResult.NotFound("任务不存在");
            entity.JobName = dto.JobName; entity.JobGroup = dto.JobGroup; entity.CronExpression = dto.CronExpression;
            entity.AssemblyName = dto.AssemblyName; entity.ClassName = dto.ClassName; entity.Description = dto.Description;
            entity.UpdateDate = now;
            // await 异步等待
            await _sysJobRepository.UpdateAsync(entity);
        }
        // else 否则分支
        else
        {
            // 创建 SysJob实例并赋给 entity
            entity = new SysJob
            {
                // null 合并操作 ??（若为 null 则使用右侧值）
                JobId = dto.JobName, JobName = dto.JobName, JobGroup = dto.JobGroup ?? "DEFAULT",
                CronExpression = dto.CronExpression, AssemblyName = dto.AssemblyName, ClassName = dto.ClassName,
                Description = dto.Description, CreateDate = now, UpdateDate = now
            };
            // await 异步等待
            await _sysJobRepository.AddAsync(entity);
        }
        // return 返回结果
        return ApiResult.Ok(SysJobDto.FromEntity(entity));
    }

    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task<ApiResult> DeleteAsync(string jobId)
    {
        // 缓存：获取值
        var entity = await _sysJobRepository.GetAsync(jobId);
        // if 条件判断
        if (entity == null) return ApiResult.NotFound("任务不存在");
        // await 异步等待
        await StopJobAsync(jobId);
        // await 异步等待
        await _sysJobRepository.DeleteAsync(entity);
        // return 返回结果
        return ApiResult.Ok();
    }

    // 方法 StartJobAsync
    // 方法：StartJobAsync
    public async Task<ApiResult> StartJobAsync(string jobId)
    {
        // 缓存：获取值
        var entity = await _sysJobRepository.GetAsync(jobId);
        // if 条件判断
        if (entity == null) return ApiResult.NotFound("任务不存在");

        var scheduler = await _schedulerFactory.GetScheduler();
        // 创建 JobKey实例并赋给 jobKey
        var jobKey = new JobKey(jobId, entity.JobGroup ?? "DEFAULT");

        // if 条件判断
        if (await scheduler.CheckExists(jobKey))
            // return 返回结果
            return ApiResult.Fail(400, "任务已在运行中");

        // 调用 IsNullOrEmpty
        var jobType = !string.IsNullOrEmpty(entity.ClassName)
            // 三元条件表达式
            ? Type.GetType($"{entity.ClassName}, {entity.AssemblyName}") ?? typeof(SampleJob)
            : typeof(SampleJob);

        // 文件/流操作：创建文件
        var job = JobBuilder.Create(jobType)
            .WithIdentity(jobKey)
            .WithDescription(entity.Description)
            .Build();

        // 文件/流操作：创建文件
        var trigger = TriggerBuilder.Create()
            // null 合并操作 ??（若为 null 则使用右侧值）
            .WithIdentity($"{jobId}.trigger", entity.JobGroup ?? "DEFAULT")
            .WithCronSchedule(entity.CronExpression)
            .Build();

        // await 异步等待
        await scheduler.ScheduleJob(job, trigger);

        entity.RunStatus = "1";
        entity.UpdateDate = DateTime.Now;
        // await 异步等待
        await _sysJobRepository.UpdateAsync(entity);
        // return 返回结果
        return ApiResult.Ok();
    }

    // 方法 StopJobAsync
    // 方法：StopJobAsync
    public async Task<ApiResult> StopJobAsync(string jobId)
    {
        // 缓存：获取值
        var entity = await _sysJobRepository.GetAsync(jobId);
        // if 条件判断
        if (entity == null) return ApiResult.NotFound("任务不存在");

        var scheduler = await _schedulerFactory.GetScheduler();
        // 创建 JobKey实例并赋给 jobKey
        var jobKey = new JobKey(jobId, entity.JobGroup ?? "DEFAULT");

        // if 条件判断
        if (await scheduler.CheckExists(jobKey))
            // await 异步等待
            await scheduler.DeleteJob(jobKey);

        entity.RunStatus = "0";
        entity.UpdateDate = DateTime.Now;
        // await 异步等待
        await _sysJobRepository.UpdateAsync(entity);
        // return 返回结果
        return ApiResult.Ok();
    }

    // 方法 RunOnceAsync
    // 方法：RunOnceAsync
    public async Task<ApiResult> RunOnceAsync(string jobId)
    {
        // 缓存：获取值
        var entity = await _sysJobRepository.GetAsync(jobId);
        // if 条件判断
        if (entity == null) return ApiResult.NotFound("任务不存在");

        var scheduler = await _schedulerFactory.GetScheduler();
        // 创建 JobKey实例并赋给 jobKey
        var jobKey = new JobKey(jobId, entity.JobGroup ?? "DEFAULT");
        // await 异步等待
        await scheduler.TriggerJob(jobKey);
        // return 返回结果
        return ApiResult.Ok();
    }

    // 方法 GetLogsAsync
    // 方法：GetLogsAsync
    public async Task<List<JobLogDto>> GetLogsAsync(string jobId)
    {
        var logs = await _jobLogRepository.FindByJobIdAsync(jobId);
        // return 返回结果
        return logs.Select(JobLogDto.FromEntity).ToList();
    }

    // 方法 InitDefaultJobsAsync
    // 方法：InitDefaultJobsAsync
    public async Task InitDefaultJobsAsync()
    {
        // if 条件判断
        if ((await _sysJobRepository.FindListAsync()).Count > 0) return;

        // 声明并初始化变量：now
        var now = DateTime.Now;
        // 创建 List实例并赋给 jobs
        var jobs = new List<SysJob>
        {
            new() { JobId = "sys_clean_log", JobName = "日志清理", JobGroup = "SYSTEM", CronExpression = "0 0 3 * * ?", AssemblyName = "JeeSiteNET.Modules.Tasks", ClassName = "JeeSiteNET.Modules.Tasks.Jobs.SampleJob", Description = "每天凌晨3点清理过期日志", RunStatus = "0", CreateDate = now, UpdateDate = now },
            new() { JobId = "sys_heartbeat", JobName = "系统心跳", JobGroup = "SYSTEM", CronExpression = "0 */5 * * * ?", AssemblyName = "JeeSiteNET.Modules.Tasks", ClassName = "JeeSiteNET.Modules.Tasks.Jobs.SampleJob", Description = "每5分钟心跳检测", RunStatus = "0", CreateDate = now, UpdateDate = now }
        };
        // foreach 遍历集合
        foreach (var job in jobs)
            // await 异步等待
            await _sysJobRepository.AddAsync(job);
    }
}
