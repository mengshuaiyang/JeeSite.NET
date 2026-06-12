<div align="right">
  <a href="Home">← 返回首页</a>
</div>

---
# Tasks 任务调度模块技术手册

## 一、模块概述

### 1.1 模块定位

Tasks 模块基于 **Quartz.NET 3.x** 为 JeeSite.NET 提供分布式任务调度能力。它把每个后台任务抽象成一条可管理、可暂停、可恢复、可触发、可删除的记录，并把任务的执行日志记录到数据库。业务方只需继承 `IJob` 编写 Execute 逻辑，就可以在管理界面上配置 Cron 表达式与参数，获得一个可长期运行的定时任务。

- 模块所在程序集：`JeeSiteNET.Modules.Tasks`（或同目录下等价命名空间）
- API 根路径：`/api/v1/tasks/`
- 前端入口：`frontend/src/views/tasks/JobList.vue`

### 1.2 核心能力

- **Cron 表达式调度**：秒/分/时/日/月/周的组合表达式
- **简单触发器 / 日历排除**（预留接口，按 Quartz 能力扩展）
- **任务持久化**：触发器与状态使用数据库 JobStore（可配置为 SqlServer/PostgreSql/MySql）
- **分布式集群**：同一 job 在多个节点上只会执行一次（通过数据库锁）
- **执行日志**：每次执行都会记录耗时、状态、异常信息
- **管理界面**：列表 + 创建/编辑 + 暂停/恢复/立即执行 + 日志查看

### 1.3 与其他模块的关系

- 依赖 `JeeSiteNET.Modules.Sys`（审计/日志/权限）
- 不依赖其他业务模块；业务方只需在自己的程序集中实现 `IJob` 即可被 Tasks 模块通过反射加载执行

## 二、核心实体

### 2.1 TaskJob（任务定义）

命名空间：`JeeSiteNET.Modules.Tasks.Domain.Entities.TaskJob`

| 字段 | 类型 | 含义 |
|------|------|------|
| JobId | long | 主键 |
| JobName | string | 任务名称，如「清理过期日志」 |
| JobGroup | string | 任务分组（Quartz 的 JobKey.Group），默认 `DEFAULT` |
| JobType | string | 任务类的 AssemblyQualifiedName / 完整类名，如 `MyNamespace.MyJob, MyAssembly` |
| CronExpression | string | Cron 表达式，如 `0 0 2 * * ?` |
| TriggerState | string | 触发器状态，枚举：`Normal`/`Paused`/`Complete`/`Error`/`Blocked`/`None` |
| IsConcurrent | bool | 是否允许并发执行（映射到 Quartz 的 `@DisallowConcurrentExecution` 或 PersistJobDataAfterExecution 语义） |
| StartTime | DateTime? | 首次可执行时间，之前不会被触发 |
| EndTime | DateTime? | 终止时间，之后不再触发 |
| Parameters | string | JSON 字符串形式的参数；执行时会被塞进 `JobDataMap` |
| Description | string | 任务描述 |
| Status | string | 记录状态（0 正常/1 停用），软开关 |
| LastRunTime | DateTime? | 最近一次执行时间（从触发器同步） |
| NextRunTime | DateTime? | 下一次触发时间（从触发器同步） |
| CreateBy | string | 创建人 |
| CreateDate | DateTime | 创建时间 |
| UpdateBy | string | 更新人 |
| UpdateDate | DateTime | 更新时间 |
| Remarks | string | 备注 |

### 2.2 TaskLog（任务执行日志）

命名空间：`JeeSiteNET.Modules.Tasks.Domain.Entities.TaskLog`

| 字段 | 类型 | 含义 |
|------|------|------|
| LogId | long | 主键 |
| JobId | long | 关联任务 |
| JobName | string | 任务名称（冗余，便于日志搜索） |
| RunTime | DateTime | 执行开始时间 |
| DurationMs | long | 执行耗时（毫秒） |
| Status | string | `Success` / `Failed` |
| ExceptionMessage | string | 异常堆栈或错误信息（失败时） |
| ServerName | string | 执行节点（机器名），用于分布式部署时排查 |
| Parameters | string | 执行时的参数快照 |

## 三、核心服务

### 3.1 TaskSchedulerService

负责与 Quartz `IScheduler` 交互：启动、停止、调度新任务、暂停、恢复、触发、删除、查询。

命名空间：`JeeSiteNET.Modules.Tasks.Application.Services.TaskSchedulerService`

| 方法 | 说明 |
|------|------|
| StartAsync(CancellationToken) | 启动调度器（由宿主在应用启动时调用一次） |
| StopAsync(CancellationToken) | 停止调度器（优雅关闭） |
| ScheduleJobAsync(TaskJob job) | 根据 `JobType` + `CronExpression` 创建并调度 JobDetail + Trigger；已存在则更新 |
| PauseJobAsync(JobKey key) | 暂停指定 JobKey 的触发器 |
| ResumeJobAsync(JobKey key) | 恢复指定 JobKey 的触发器 |
| TriggerJobAsync(JobKey key) | 立即触发一次（忽略 Cron） |
| DeleteJobAsync(JobKey key) | 删除 JobDetail 与关联的 Trigger |
| GetAllJobsAsync() | 获取所有任务运行状态快照（用于展示） |
| GetJobDetailAsync(JobKey key) | 获取单条任务运行状态 |

**调度要点**：

- JobKey = `new JobKey(job.JobName, job.JobGroup)`
- 通过 `Type.GetType(job.JobType)` 加载 Job 类型；如果任务类在其他程序集，需要确保其已被加载（或显式 `Assembly.Load`）
- JobDataMap 内容来自 `Parameters`（JSON → 字典）
- 全局监听 `IJobListener`，在 `JobWasExecuted` 中将执行结果写入 `TaskLog`

### 3.2 TaskJobService

管理 TaskJob 表的 CRUD，并同步调度器。

命名空间：`JeeSiteNET.Modules.Tasks.Application.Services.TaskJobService`

| 方法 | 说明 |
|------|------|
| GetPagedListAsync(keyword, status, pageNo, pageSize) | 任务分页列表 |
| GetByIdAsync(long id) | 获取任务详情 |
| CreateAsync(TaskJob job) | 新增任务并同步调度器（若 Status=0 则立即 Schedule） |
| UpdateAsync(TaskJob job) | 更新任务并同步调度器（删除旧 Trigger，重建） |
| DeleteAsync(long id) | 删除任务（从表与调度器都移除） |
| PauseAsync(long id) | 暂停任务（更新表 + 调用 `PauseJobAsync`） |
| ResumeAsync(long id) | 恢复任务（更新表 + 调用 `ResumeJobAsync`） |
| RunAsync(long id) | 立即执行一次（调用 `TriggerJobAsync`） |

### 3.3 TaskLogService

命名空间：`JeeSiteNET.Modules.Tasks.Application.Services.TaskLogService`

| 方法 | 说明 |
|------|------|
| GetPagedListAsync(jobId, pageNo, pageSize) | 指定任务的执行日志分页 |
| CreateAsync(TaskLog log) | 写日志（由 JobListener 在任务结束后调用） |
| CleanupAsync(DateTime olderThan) | 清理指定时间之前的日志（可被「清理任务日志」任务调用） |

## 四、控制器与 API

控制器：`JeeSiteNET.Modules.Tasks.Controllers.JobController`

| 方法 | HTTP | 路由 | 说明 |
|------|------|------|------|
| GetListAsync | GET | `/api/v1/tasks/job/list` | 任务分页列表（查询参数：keyword/status/pageNo/pageSize） |
| GetAsync | GET | `/api/v1/tasks/job/{id}` | 任务详情 |
| CreateAsync | POST | `/api/v1/tasks/job` | 创建任务 |
| UpdateAsync | PUT | `/api/v1/tasks/job/{id}` | 编辑任务 |
| DeleteAsync | DELETE | `/api/v1/tasks/job/{id}` | 删除任务 |
| PauseAsync | POST | `/api/v1/tasks/job/{id}/pause` | 暂停 |
| ResumeAsync | POST | `/api/v1/tasks/job/{id}/resume` | 恢复 |
| RunAsync | POST | `/api/v1/tasks/job/{id}/run` | 立即执行 |
| GetLogsAsync | GET | `/api/v1/tasks/job/{id}/logs` | 执行日志列表 |

### 4.1 Create / Update 请求体示例

```json
{
  "jobName": "CleanupLogs",
  "jobGroup": "DEFAULT",
  "jobType": "JeeSiteNET.Modules.Tasks.Jobs.CleanupLogsJob, JeeSiteNET.Modules.Tasks",
  "cronExpression": "0 0 2 * * ?",
  "isConcurrent": false,
  "startTime": "2026-01-01T00:00:00Z",
  "endTime": null,
  "parameters": "{\"days\": 30}",
  "description": "每天凌晨 2 点清理 30 天之前的日志",
  "status": "0"
}
```

## 五、开发自定义任务

### 5.1 实现 IJob

```csharp
using Quartz;

namespace MyCompany.MyModule.Jobs
{
    public class MyJob : IJob
    {
        // 可注入任意 Service（需要 DI 支持）
        private readonly IMyService _myService;
        private readonly ILogger<MyJob> _logger;

        public MyJob(IMyService myService, ILogger<MyJob> logger)
        {
            _myService = myService;
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            // 从 JobDataMap 读取参数
            var days = context.MergedJobDataMap.GetInt("days");
            var param1 = context.MergedJobDataMap.GetString("param1");

            try
            {
                // 业务逻辑
                await _myService.DoWorkAsync(days);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MyJob 执行失败");
                // 抛出异常时会被 JobListener 记录到 TaskLog
                throw;
            }

            return Task.CompletedTask;
        }
    }
}
```

**可选 Attribute**：

- `[DisallowConcurrentExecution]`：禁止同一 JobDetail 并发执行
- `[PersistJobDataAfterExecution]`：在成功执行后持久化 JobDataMap 的更改

### 5.2 通过管理界面登记

进入「系统监控 → 任务调度 → 新增任务」，填写：

| 字段 | 示例 |
|------|------|
| 任务名称 | `MyJob` |
| 任务分组 | `DEFAULT` |
| 任务类型（JobType） | `MyCompany.MyModule.Jobs.MyJob, MyAssembly` |
| Cron 表达式 | `0 0/5 * * * ?` |
| 参数（Parameters） | `{ "days": 30 }` |
| 是否并发 | `否` |
| 状态 | `正常` |

保存后 TaskSchedulerService 会把任务注册到 Quartz，下一次命中 Cron 时执行。

### 5.3 任务参数的读取约定

`Parameters` 字段被要求是合法 JSON（对象形式），TaskSchedulerService 在创建 `JobDetail` 时会把它展开为 `JobDataMap` 的键值对。执行时在 `IJobExecutionContext.MergedJobDataMap` 中读取。

## 六、常用 Cron 表达式（Quartz 格式）

Quartz 的 Cron 共 **6 或 7 段**：`秒 分 时 日 月 周 [年]`。

> 注意：`日` 与 `周` 其中之一必须是 `?`（不指定），否则非法。

| 含义 | 表达式 |
|------|--------|
| 每 5 分钟 | `0 0/5 * * * ?` |
| 每天凌晨 2 点 | `0 0 2 * * ?` |
| 周一至周五 00:00 | `0 0 0 ? * MON-FRI` |
| 每月 1 号 00:00 | `0 0 0 1 * ?` |
| 每月最后一天 00:00 | `0 0 0 L * ?` |
| 每天 8:00-20:00 每隔 2 小时 | `0 0 8-20/2 * * ?` |
| 每年 1 月 1 日 00:00 | `0 0 0 1 1 ? *` |

特殊字符速查：

| 字符 | 含义 |
|------|------|
| `*` | 任意值 |
| `?` | 不指定（用于日/周二选一） |
| `-` | 范围，如 `10-20` |
| `,` | 枚举，如 `1,3,5` |
| `/` | 步长，如 `0/5`（从 0 开始每 5） |
| `L` | 最后（用于日=最后一天，或周=最后一周 X 天） |
| `W` | 最近工作日 |
| `#` | 第几个周几，如 `6#3` 表示第 3 个周五 |

## 七、注意事项与最佳实践

1. **任务类必须可被反射加载**：`JobType` 写完整类名 + 程序集名（`Type.AssemblyQualifiedName` 的形式最稳）。任务类所在程序集必须在执行目录中。
2. **任务类构造函数**：推荐使用 DI 注入；至少保留一个公开的无参构造函数（以防 DI 未被正确配置时 Quartz 的默认工厂仍可创建实例）。
3. **异常处理**：
   - 业务异常在 `Execute` 内捕获并记录；如需重试可以 `throw new JobExecutionException(refireImmediately: true)`。
   - 未处理异常会被全局 JobListener 写入 `TaskLog` 并将触发器标记为 `Error`。
4. **分布式部署**：
   - 所有节点必须使用 **相同的 JobStore 配置**（同一个数据库与表前缀）。
   - `quartz.scheduler.instanceName` 相同，`quartz.scheduler.instanceId` 使用 `AUTO`。
   - 多节点下同一任务只会执行一次；节点间通过数据库行锁互相排斥。
5. **日志**：任务内使用标准 `ILogger<T>` 写结构化日志；同时每次执行都会在 `TaskLog` 保留一条摘要。
6. **并发控制**：
   - 为每个 Job 类打上 `[DisallowConcurrentExecution]` 防止同任务重叠。
   - 对跨 Job 的共享资源通过 `IsConcurrent = false` 在业务层做标识，或在 Job 里自己加锁。
7. **定时任务的幂等**：任务被多次触发（手动/定时）时应保证结果一致；避免把状态完全依赖于「执行次数」。
8. **长任务**：若任务超过 30 分钟仍未结束，考虑拆分为多个阶段或使用 `CancellationToken` 响应宿主的停止请求。
9. **Cron 调试**：管理界面提供「计算下 N 次触发时间」按钮；或使用 [CronMaker](https://www.cronmaker.com/) 在线工具。
10. **清理**：通过内置的 `CleanupLogsJob` 或自定义任务定期清理 `TaskLog`，避免无限增长。

## 八、内置任务列表

以下任务通常由模块首次安装时在 `TasksModuleInstaller` / 数据库初始化脚本中插入（Status=1，即默认不开启，按需手动启用）。

| 任务名称 | 任务类 | 建议 Cron | 作用 |
|----------|--------|-----------|------|
| 清理过期日志 | `CleanupLogsJob` | `0 0 2 * * ?` | 每天 02:00 删除 `sys_log`/`task_log` 中超过 30 天的记录（可通过 Parameters 调整天数） |
| 刷新字典缓存 | `RefreshDictCacheJob` | `0 0 4 * * ?` | 每天 04:00 重建 `DictType/DictData` 的缓存 |
| 统计访问数据 | `StatsCmsVisitJob` | `0 0 * * * ?` | 每小时汇总 CMS 访问量（按天聚合到统计表） |
| 推送到期消息 | `PushDueMessageJob` | `0 0/10 * * * ?` | 每 10 分钟扫描到期消息并推送（站内信/邮件） |
| 清理临时文件 | `CleanupTempFilesJob` | `0 30 3 * * ?` | 每天 03:30 删除用户上传的过期临时文件 |

> 任务类的命名空间与类名以实际项目为准；管理界面可以直接搜索 `JobType` 快速定位。

## 九、模块主要文件结构

```
modules/JeeSiteNET.Modules.Tasks/
├─ Application/
│   ├─ DTOs/
│   │   ├─ TaskJobDto.cs
│   │   └─ TaskLogDto.cs
│   ├─ Jobs/
│   │   ├─ CleanupLogsJob.cs
│   │   ├─ RefreshDictCacheJob.cs
│   │   ├─ StatsCmsVisitJob.cs
│   │   └─ PushDueMessageJob.cs
│   └─ Services/
│       ├─ TaskSchedulerService.cs
│       ├─ TaskJobService.cs
│       └─ TaskLogService.cs
├─ Controllers/
│   └─ JobController.cs
├─ Domain/
│   ├─ Entities/
│   │   ├─ TaskJob.cs
│   │   └─ TaskLog.cs
│   └─ Interfaces/
│       ├─ ITaskJobRepository.cs
│       └─ ITaskLogRepository.cs
├─ Infrastructure/
│   ├─ EntityConfigurations/
│   │   ├─ TaskJobConfiguration.cs
│   │   └─ TaskLogConfiguration.cs
│   ├─ Listener/
│   │   └─ TaskJobListener.cs
│   └─ Repositories/
│       ├─ TaskJobRepository.cs
│       └─ TaskLogRepository.cs
├─ TasksModuleInstaller.cs
└─ JeeSiteNET.Modules.Tasks.csproj

frontend/src/
├─ api/
│   └─ tasksJob.ts
└─ views/tasks/
    └─ JobList.vue
```

### 关键配置项（`appsettings.json`）

```json
{
  "Quartz": {
    "scheduler": {
      "instanceName": "JeeSiteScheduler",
      "instanceId": "AUTO"
    },
    "threadPool": {
      "type": "Quartz.Simpl.SimpleThreadPool, Quartz",
      "threadCount": 10,
      "threadPriority": "Normal"
    },
    "jobStore": {
      "type": "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
      "driverDelegateType": "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz",
      "dataSource": "default",
      "tablePrefix": "QRTZ_",
      "useProperties": "false",
      "misfireThreshold": "60000"
    },
    "dataSource": {
      "default": {
        "connectionString": "Server=.;Database=jeesite_net;Trusted_Connection=True;TrustServerCertificate=True",
        "provider": "SqlServer"
      }
    }
  }
}
```
---

<div align="center">
  <small>本文档最后更新: 2026-06-12 · JeeSite.NET Wiki</small>
</div>