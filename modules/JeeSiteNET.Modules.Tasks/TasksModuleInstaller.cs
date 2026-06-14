    // 引入 JeeSiteNET.Core.Modules 命名空间
// 引入命名空间：JeeSiteNET.Core.Modules
using JeeSiteNET.Core.Modules;
    // 引入 JeeSiteNET.Modules.Tasks.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Tasks.Application.Services
using JeeSiteNET.Modules.Tasks.Application.Services;
    // 引入 JeeSiteNET.Modules.Tasks.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Tasks.Domain.Interfaces
using JeeSiteNET.Modules.Tasks.Domain.Interfaces;
    // 引入 JeeSiteNET.Modules.Tasks.Infrastructure.Repositories 命名空间
// 引入命名空间：JeeSiteNET.Modules.Tasks.Infrastructure.Repositories
using JeeSiteNET.Modules.Tasks.Infrastructure.Repositories;
    // 引入 Microsoft.Extensions.Configuration 命名空间
// 引入命名空间：Microsoft.Extensions.Configuration
using Microsoft.Extensions.Configuration;
    // 引入 Microsoft.Extensions.DependencyInjection 命名空间
// 引入命名空间：Microsoft.Extensions.DependencyInjection
using Microsoft.Extensions.DependencyInjection;
    // 引入 Quartz 命名空间
// 引入命名空间：Quartz
using Quartz;

// 定义 JeeSiteNET.Modules.Tasks 命名空间
// 定义命名空间：JeeSiteNET.Modules.Tasks
namespace JeeSiteNET.Modules.Tasks;

[ModuleDescription(Code = "Tasks", Name = "定时任务调度模块", Version = "1.0.0")]
// 定义class TasksModuleInstaller
// 定义类：TasksModuleInstaller
public class TasksModuleInstaller : IModuleInstaller
{
    // 方法 ConfigureServices
    // 方法：ConfigureServices
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISysJobRepository, SysJobRepository>();
        services.AddScoped<IJobLogRepository, JobLogRepository>();
        services.AddScoped<SchedulerService>();

        services.AddQuartz(q =>
        {
            q.UseSimpleTypeLoader();
        });
        services.AddQuartzHostedService(options =>
        {
            // 设置配置选项
            options.WaitForJobsToComplete = true;
        });
    }
}
