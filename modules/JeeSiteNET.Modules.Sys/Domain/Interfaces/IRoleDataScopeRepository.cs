using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

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
