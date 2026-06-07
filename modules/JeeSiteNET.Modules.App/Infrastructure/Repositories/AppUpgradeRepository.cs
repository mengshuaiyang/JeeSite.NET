using JeeSiteNET.Modules.App.Domain.Entities;
using JeeSiteNET.Modules.App.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.App.Infrastructure.Repositories;

public class AppUpgradeRepository : IAppUpgradeRepository
{
    private readonly JeeSiteDbContext _db;
    public AppUpgradeRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<AppUpgrade> Query() => _db.Set<AppUpgrade>().AsNoTracking();
    public async Task<AppUpgrade?> GetAsync(object id) => await _db.Set<AppUpgrade>().FindAsync(id);
    public async Task<List<AppUpgrade>> FindListAsync() => await _db.Set<AppUpgrade>().AsNoTracking().ToListAsync();
    public async Task AddAsync(AppUpgrade entity) { _db.Set<AppUpgrade>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(AppUpgrade entity) { _db.Set<AppUpgrade>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(AppUpgrade entity) { _db.Set<AppUpgrade>().Remove(entity); await _db.SaveChangesAsync(); }
}
