    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

// 定义接口 IRoleFieldScopeRepository
// 定义接口：IRoleFieldScopeRepository
public interface IRoleFieldScopeRepository
{
    IQueryable<RoleFieldScope> Query();
    Task<RoleFieldScope?> GetAsync(string id);
    Task<List<RoleFieldScope>> GetByRoleMenuAsync(string roleCode, string menuCode);
    Task AddAsync(RoleFieldScope entity);
    Task UpdateAsync(RoleFieldScope entity);
    Task DeleteAsync(string id);
    Task SaveChangesAsync();
}
