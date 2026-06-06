using JeeSiteNET.Core;
using JeeSiteNET.Modules.Tasks.Domain.Entities;

namespace JeeSiteNET.Modules.Tasks.Domain.Interfaces;

public interface ISysJobRepository : IRepository<SysJob>
{
    Task<List<SysJob>> FindRunningJobsAsync();
}
