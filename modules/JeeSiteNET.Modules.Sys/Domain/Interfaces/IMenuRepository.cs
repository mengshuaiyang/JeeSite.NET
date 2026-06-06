using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

public interface IMenuRepository : IRepository<Menu>
{
    Task<List<string>> GetPermissionsByRoleCodesAsync(List<string> roleCodes);
}
