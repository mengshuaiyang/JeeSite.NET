using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

public interface ITenantRepository : IRepository<Tenant>
{
    Task<PageResult<Tenant>> FindPageAsync(PageRequest<Tenant> request);
}
