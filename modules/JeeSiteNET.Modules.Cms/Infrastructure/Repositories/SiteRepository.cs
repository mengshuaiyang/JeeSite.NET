using JeeSiteNET.Core;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Cms.Infrastructure.Repositories;

public class SiteRepository : ISiteRepository
{
    private readonly JeeSiteDbContext _db;
    public SiteRepository(JeeSiteDbContext db) => _db = db;
    public IQueryable<Site> Query() => _db.Set<Site>().AsNoTracking();
    public async Task<Site?> GetAsync(object id) => await _db.Set<Site>().FindAsync(id);
    public async Task<List<Site>> FindListAsync() => await _db.Set<Site>().AsNoTracking().ToListAsync();
    public async Task AddAsync(Site entity) { _db.Set<Site>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(Site entity) { _db.Set<Site>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(Site entity) { _db.Set<Site>().Remove(entity); await _db.SaveChangesAsync(); }
}
