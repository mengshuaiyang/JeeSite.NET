using JeeSiteNET.Core;
using JeeSiteNET.Modules.Cms.Domain.Entities;

namespace JeeSiteNET.Modules.Cms.Domain.Interfaces;

public interface IVisitLogRepository : IRepository<VisitLog>
{
    Task<PageResult<VisitLog>> FindPageAsync(PageRequest<VisitLog> request);
}
