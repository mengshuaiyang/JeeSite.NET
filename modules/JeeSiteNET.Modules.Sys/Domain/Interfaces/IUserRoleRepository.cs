    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

// 定义接口 IUserRoleRepository
// 定义接口：IUserRoleRepository
public interface IUserRoleRepository
{
    Task<List<string>> GetRoleCodesByUserAsync(string userCode);
    Task<List<string>> GetUserCodesByRoleAsync(string roleCode);
    Task SaveUserRolesAsync(string userCode, List<string> roleCodes);
    Task SaveRoleUsersAsync(string roleCode, List<string> userCodes);
    Task DeleteByUserAsync(string userCode);
}
