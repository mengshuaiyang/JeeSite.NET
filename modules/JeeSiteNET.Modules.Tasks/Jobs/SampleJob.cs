using Microsoft.Extensions.Logging;
using Quartz;

namespace JeeSiteNET.Modules.Tasks.Jobs;

[DisallowConcurrentExecution]
public class SampleJob : IJob
{
    private readonly ILogger<SampleJob> _logger;

    public SampleJob(ILogger<SampleJob> logger) => _logger = logger;

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("SampleJob 开始执行: {Time}", DateTime.Now);
        await Task.CompletedTask;
        _logger.LogInformation("SampleJob 执行完成: {Time}", DateTime.Now);
    }
}
