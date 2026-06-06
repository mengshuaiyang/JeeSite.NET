using JeeSiteNET.Core;
using JeeSiteNET.Modules.Bpm.Domain.Entities;

namespace JeeSiteNET.Modules.Bpm.Domain.Interfaces;

public interface IWorkflowFormRepository : IRepository<WorkflowForm>
{
    Task<WorkflowForm?> FindByBusinessKeyAsync(string businessKey);
}
