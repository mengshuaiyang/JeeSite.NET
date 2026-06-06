using JeeSiteNET.Core;
using JeeSiteNET.Modules.Tasks.Domain.Entities;

namespace JeeSiteNET.Modules.Tasks.Domain.Interfaces;

public interface IJobLogRepository : IRepository<JobLog>
{
    Task<List<JobLog>> FindByJobIdAsync(string jobId, int limit = 50);
}
