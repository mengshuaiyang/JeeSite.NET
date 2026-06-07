using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class RoleFieldScopeRepository : IRoleFieldScopeRepository
{
    private readonly DbContext _db;

    public RoleFieldScopeRepository(DbContext db)
    {
        _db = db;
    }

    public IQueryable<RoleFieldScope> Query() => _db.Set<RoleFieldScope>().AsNoTracking();

    public async Task<RoleFieldScope?> GetAsync(string id)
        => await _db.Set<RoleFieldScope>().FindAsync(id);

    public async Task<List<RoleFieldScope>> GetByRoleMenuAsync(string roleCode, string menuCode)
        => await _db.Set<RoleFieldScope>()
            .Where(e => e.RoleCode == roleCode && e.MenuCode == menuCode)
            .ToListAsync();

    public async Task AddAsync(RoleFieldScope entity)
        => await _db.Set<RoleFieldScope>().AddAsync(entity);

    public Task UpdateAsync(RoleFieldScope entity)
    {
        _db.Set<RoleFieldScope>().Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await _db.Set<RoleFieldScope>().FindAsync(id);
        if (entity != null) _db.Set<RoleFieldScope>().Remove(entity);
    }

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
