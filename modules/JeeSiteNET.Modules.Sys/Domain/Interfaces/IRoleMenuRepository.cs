using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

public interface IRoleMenuRepository
{
    Task<List<string>> GetMenuCodesByRoleAsync(string roleCode);
    Task SaveRoleMenusAsync(string roleCode, List<string> menuCodes);
    Task<List<string>> GetMenuCodesByRolesAsync(List<string> roleCodes);
}
