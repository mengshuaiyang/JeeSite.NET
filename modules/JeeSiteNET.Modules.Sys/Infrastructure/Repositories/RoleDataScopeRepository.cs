using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class RoleDataScopeRepository : IRoleDataScopeRepository
{
    private readonly JeeSiteDbContext _db;

    public RoleDataScopeRepository(JeeSiteDbContext db)
    {
        _db = db;
    }

    public IQueryable<RoleDataScope> Query() => _db.Set<RoleDataScope>().AsNoTracking();

    public async Task<RoleDataScope?> GetAsync(string roleCode, string menuCode)
    {
        return await _db.Set<RoleDataScope>()
            .FirstOrDefaultAsync(e => e.RoleCode == roleCode && e.MenuCode == menuCode);
    }

    public async Task<List<RoleDataScope>> FindListAsync() => await _db.Set<RoleDataScope>().ToListAsync();

    public async Task<List<RoleDataScope>> FindByRoleAsync(string roleCode)
    {
        return await _db.Set<RoleDataScope>()
            .Where(e => e.RoleCode == roleCode)
            .ToListAsync();
    }

    public async Task AddAsync(RoleDataScope entity)
    {
        await _db.Set<RoleDataScope>().AddAsync(entity);
    }

    public async Task UpdateAsync(RoleDataScope entity)
    {
        _db.Set<RoleDataScope>().Update(entity);
    }

    public async Task DeleteAsync(string roleCode, string menuCode)
    {
        var entity = await _db.Set<RoleDataScope>()
            .FirstOrDefaultAsync(e => e.RoleCode == roleCode && e.MenuCode == menuCode);
        if (entity != null)
        {
            _db.Set<RoleDataScope>().Remove(entity);
        }
    }

    public async Task DeleteByRoleAsync(string roleCode)
    {
        var entities = await _db.Set<RoleDataScope>()
            .Where(e => e.RoleCode == roleCode)
            .ToListAsync();
        _db.Set<RoleDataScope>().RemoveRange(entities);
    }

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
