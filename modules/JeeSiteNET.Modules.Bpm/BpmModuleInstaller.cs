    // 引入 Elsa.Extensions 命名空间
// 引入命名空间：Elsa.Extensions
using Elsa.Extensions;
    // 引入 JeeSiteNET.Core.Modules 命名空间
// 引入命名空间：JeeSiteNET.Core.Modules
using JeeSiteNET.Core.Modules;
    // 引入 JeeSiteNET.Modules.Bpm.Activities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Activities
using JeeSiteNET.Modules.Bpm.Activities;
    // 引入 JeeSiteNET.Modules.Bpm.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Application.Services
using JeeSiteNET.Modules.Bpm.Application.Services;
    // 引入 JeeSiteNET.Modules.Bpm.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Domain.Interfaces
using JeeSiteNET.Modules.Bpm.Domain.Interfaces;
    // 引入 JeeSiteNET.Modules.Bpm.Infrastructure.Repositories 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Infrastructure.Repositories
using JeeSiteNET.Modules.Bpm.Infrastructure.Repositories;
    // 引入 Microsoft.Extensions.Configuration 命名空间
// 引入命名空间：Microsoft.Extensions.Configuration
using Microsoft.Extensions.Configuration;
    // 引入 Microsoft.Extensions.DependencyInjection 命名空间
// 引入命名空间：Microsoft.Extensions.DependencyInjection
using Microsoft.Extensions.DependencyInjection;

// 定义 JeeSiteNET.Modules.Bpm 命名空间
// 定义命名空间：JeeSiteNET.Modules.Bpm
namespace JeeSiteNET.Modules.Bpm;

[ModuleDescription(Code = "Bpm", Name = "工作流引擎模块", Version = "1.0.0")]
// 定义class BpmModuleInstaller
// 定义类：BpmModuleInstaller
public class BpmModuleInstaller : IModuleInstaller
{
    // 方法 ConfigureServices
    // 方法：ConfigureServices
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IApprovalRecordRepository, ApprovalRecordRepository>();
        services.AddScoped<IWorkflowFormRepository, WorkflowFormRepository>();
        services.AddScoped<ILeaveRepository, LeaveRepository>();
        services.AddScoped<BpmService>();
        services.AddScoped<LeaveService>();

        services.AddElsa(elsa =>
        {
            elsa.UseWorkflowManagement();
            elsa.UseWorkflowRuntime();
            elsa.AddActivitiesFrom<BpmModuleInstaller>();
        });
    }
}
