using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class ConfigRepository : IConfigRepository
{
    private readonly JeeSiteDbContext _db;
    public ConfigRepository(JeeSiteDbContext db) => _db = db;
    public IQueryable<Config> Query() => _db.Set<Config>().AsNoTracking();
    public async Task<Config?> GetAsync(object id) => await _db.Set<Config>().FindAsync(id);
    public async Task<List<Config>> FindListAsync() => await _db.Set<Config>().AsNoTracking().ToListAsync();
    public async Task AddAsync(Config entity) { await _db.Set<Config>().AddAsync(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(Config entity) { _db.Set<Config>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(Config entity) { _db.Set<Config>().Remove(entity); await _db.SaveChangesAsync(); }
}
