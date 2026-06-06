using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

public interface IUserRoleRepository
{
    Task<List<string>> GetRoleCodesByUserAsync(string userCode);
    Task SaveUserRolesAsync(string userCode, List<string> roleCodes);
    Task DeleteByUserAsync(string userCode);
}
