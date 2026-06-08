using Elsa.Extensions;
using JeeSiteNET.Core.Modules;
using JeeSiteNET.Modules.Bpm.Activities;
using JeeSiteNET.Modules.Bpm.Application.Services;
using JeeSiteNET.Modules.Bpm.Domain.Interfaces;
using JeeSiteNET.Modules.Bpm.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JeeSiteNET.Modules.Bpm;

[ModuleDescription(Code = "Bpm", Name = "工作流引擎模块", Version = "1.0.0")]
public class BpmModuleInstaller : IModuleInstaller
{
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
