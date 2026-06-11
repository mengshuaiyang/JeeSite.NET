using JeeSiteNET.Core;
using JeeSiteNET.Modules.Cms.Domain.Entities;

namespace JeeSiteNET.Modules.Cms.Domain.Interfaces;

public interface IReportRepository : IRepository<Report>
{
    Task<PageResult<Report>> FindPageAsync(PageRequest<Report> request);
}
