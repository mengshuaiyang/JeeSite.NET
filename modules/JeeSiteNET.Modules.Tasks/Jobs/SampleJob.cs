    // 引入 Microsoft.Extensions.Logging 命名空间
// 引入命名空间：Microsoft.Extensions.Logging
using Microsoft.Extensions.Logging;
    // 引入 Quartz 命名空间
// 引入命名空间：Quartz
using Quartz;

// 定义 JeeSiteNET.Modules.Tasks.Jobs 命名空间
// 定义命名空间：JeeSiteNET.Modules.Tasks.Jobs
namespace JeeSiteNET.Modules.Tasks.Jobs;

[DisallowConcurrentExecution]
// 定义class SampleJob
// 定义类：SampleJob
public class SampleJob : IJob
{
    // 字段 _logger
    // 字段：_logger
    private readonly ILogger<SampleJob> _logger;

    // 构造函数 SampleJob
    // 构造函数：SampleJob
    public SampleJob(ILogger<SampleJob> logger) => _logger = logger;

    // 方法 Execute
    // 方法：Execute
    public async Task Execute(IJobExecutionContext context)
    {
        // 日志：记录信息
        _logger.LogInformation("SampleJob 开始执行: {Time}", DateTime.Now);
        // await 异步等待
        await Task.CompletedTask;
        // 日志：记录信息
        _logger.LogInformation("SampleJob 执行完成: {Time}", DateTime.Now);
    }
}
