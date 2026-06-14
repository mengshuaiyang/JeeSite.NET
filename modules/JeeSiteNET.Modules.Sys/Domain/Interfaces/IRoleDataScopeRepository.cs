    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

// 定义接口 IRoleDataScopeRepository
// 定义接口：IRoleDataScopeRepository
public interface IRoleDataScopeRepository
{
    IQueryable<RoleDataScope> Query();
    Task<RoleDataScope?> GetAsync(string roleCode, string menuCode);
    Task<List<RoleDataScope>> FindListAsync();
    Task<List<RoleDataScope>> FindByRoleAsync(string roleCode);
    Task AddAsync(RoleDataScope entity);
    Task UpdateAsync(RoleDataScope entity);
    Task DeleteAsync(string roleCode, string menuCode);
    Task DeleteByRoleAsync(string roleCode);
    Task SaveChangesAsync();
}
