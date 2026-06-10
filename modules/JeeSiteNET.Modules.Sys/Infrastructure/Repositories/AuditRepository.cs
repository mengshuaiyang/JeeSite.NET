using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class AuditRepository : IAuditRepository
{
    private readonly JeeSiteDbContext _db;
    public AuditRepository(JeeSiteDbContext db) => _db = db;
    public IQueryable<Audit> Query() => _db.Set<Audit>().AsNoTracking();
    public async Task<Audit?> GetAsync(object id) => await _db.Set<Audit>().FindAsync(id);
    public async Task<List<Audit>> FindListAsync() => await _db.Set<Audit>().AsNoTracking().ToListAsync();
    public async Task AddAsync(Audit entity) { await _db.Set<Audit>().AddAsync(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(Audit entity) { _db.Set<Audit>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(Audit entity) { _db.Set<Audit>().Remove(entity); await _db.SaveChangesAsync(); }
}
