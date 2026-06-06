using JeeSiteNET.Core.Modules;
using JeeSiteNET.Modules.Tasks.Application.Services;
using JeeSiteNET.Modules.Tasks.Domain.Interfaces;
using JeeSiteNET.Modules.Tasks.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace JeeSiteNET.Modules.Tasks;

[ModuleDescription(Code = "Tasks", Name = "定时任务调度模块", Version = "1.0.0")]
public class TasksModuleInstaller : IModuleInstaller
{
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
            options.WaitForJobsToComplete = true;
        });
    }
}
