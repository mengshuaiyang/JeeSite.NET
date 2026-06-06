using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

public interface IRoleRepository : IRepository<Role>
{
    Task<List<string>> GetRoleCodesByUserAsync(string userCode);
}
