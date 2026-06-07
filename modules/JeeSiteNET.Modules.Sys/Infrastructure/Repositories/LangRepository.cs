using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class LangRepository : ILangRepository
{
    private readonly JeeSiteDbContext _db;
    public LangRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<Lang> Query() => _db.Set<Lang>().AsNoTracking();
    public async Task<Lang?> GetAsync(object id) => await _db.Set<Lang>().FindAsync(id);
    public async Task<List<Lang>> FindListAsync() => await _db.Set<Lang>().AsNoTracking().ToListAsync();
    public async Task AddAsync(Lang entity) { _db.Set<Lang>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(Lang entity) { _db.Set<Lang>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(Lang entity) { _db.Set<Lang>().Remove(entity); await _db.SaveChangesAsync(); }
}
