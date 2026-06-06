using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class ModuleRepository : IModuleRepository
{
    private readonly JeeSiteDbContext _db;
    public ModuleRepository(JeeSiteDbContext db) => _db = db;
    public IQueryable<Module> Query() => _db.Set<Module>().AsNoTracking();
    public async Task<Module?> GetAsync(object id) => await _db.Set<Module>().FindAsync(id);
    public async Task<List<Module>> FindListAsync() => await _db.Set<Module>().AsNoTracking().ToListAsync();
    public async Task AddAsync(Module entity) { await _db.Set<Module>().AddAsync(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(Module entity) { _db.Set<Module>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(Module entity) { _db.Set<Module>().Remove(entity); await _db.SaveChangesAsync(); }
}
