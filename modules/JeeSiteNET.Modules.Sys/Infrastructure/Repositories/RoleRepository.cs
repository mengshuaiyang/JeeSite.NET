using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly JeeSiteDbContext _db;

    public RoleRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<Role> Query() => _db.Set<Role>().AsNoTracking();

    public async Task<Role?> GetAsync(object id)
        => await _db.Set<Role>().FindAsync(id);

    public async Task<List<Role>> FindListAsync()
        => await _db.Set<Role>().AsNoTracking().ToListAsync();

    public async Task AddAsync(Role entity)
    {
        await _db.Set<Role>().AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Role entity)
    {
        _db.Set<Role>().Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Role entity)
    {
        _db.Set<Role>().Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<List<string>> GetRoleCodesByUserAsync(string userCode)
        => await _db.Set<Role>()
            .Where(r => r.Status == "0")
            .Select(r => r.RoleCode)
            .ToListAsync();
}
