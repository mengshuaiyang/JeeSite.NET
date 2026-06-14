    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

// 定义接口 IRoleMenuRepository
// 定义接口：IRoleMenuRepository
public interface IRoleMenuRepository
{
    Task<List<string>> GetMenuCodesByRoleAsync(string roleCode);
    Task SaveRoleMenusAsync(string roleCode, List<string> menuCodes);
    Task<List<string>> GetMenuCodesByRolesAsync(List<string> roleCodes);
}
