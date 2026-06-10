using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

public interface IUserRoleRepository
{
    Task<List<string>> GetRoleCodesByUserAsync(string userCode);
    Task<List<string>> GetUserCodesByRoleAsync(string roleCode);
    Task SaveUserRolesAsync(string userCode, List<string> roleCodes);
    Task SaveRoleUsersAsync(string roleCode, List<string> userCodes);
    Task DeleteByUserAsync(string userCode);
}
