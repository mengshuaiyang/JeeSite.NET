using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class MenuRepository : IMenuRepository
{
    private readonly JeeSiteDbContext _db;

    public MenuRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<Menu> Query() => _db.Set<Menu>().AsNoTracking();

    public async Task<Menu?> GetAsync(object id)
        => await _db.Set<Menu>().FindAsync(id);

    public async Task<List<Menu>> FindListAsync()
        => await _db.Set<Menu>().AsNoTracking().ToListAsync();

    public async Task AddAsync(Menu entity)
    {
        await _db.Set<Menu>().AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Menu entity)
    {
        _db.Set<Menu>().Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Menu entity)
    {
        _db.Set<Menu>().Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<List<string>> GetPermissionsByRoleCodesAsync(List<string> roleCodes)
        => await _db.Set<Menu>()
            .Where(m => m.Status == "0" && m.Permission != null)
            .Select(m => m.Permission!)
            .ToListAsync();
}
