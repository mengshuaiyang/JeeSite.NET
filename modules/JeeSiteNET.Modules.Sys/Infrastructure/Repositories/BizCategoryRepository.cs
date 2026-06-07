using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class BizCategoryRepository : IBizCategoryRepository
{
    private readonly JeeSiteDbContext _db;
    public BizCategoryRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<BizCategory> Query() => _db.Set<BizCategory>().AsNoTracking();
    public async Task<BizCategory?> GetAsync(object id) => await _db.Set<BizCategory>().FindAsync(id);
    public async Task<List<BizCategory>> FindListAsync() => await _db.Set<BizCategory>().AsNoTracking().ToListAsync();
    public async Task AddAsync(BizCategory entity) { _db.Set<BizCategory>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(BizCategory entity) { _db.Set<BizCategory>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(BizCategory entity) { _db.Set<BizCategory>().Remove(entity); await _db.SaveChangesAsync(); }
}
