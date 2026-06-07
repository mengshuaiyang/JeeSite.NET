using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

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
