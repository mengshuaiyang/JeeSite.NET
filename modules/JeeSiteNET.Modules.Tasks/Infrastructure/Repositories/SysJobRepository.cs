using JeeSiteNET.Core;
using JeeSiteNET.Modules.Tasks.Domain.Entities;
using JeeSiteNET.Modules.Tasks.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Tasks.Infrastructure.Repositories;

public class SysJobRepository : ISysJobRepository
{
    private readonly JeeSiteDbContext _db;
    public SysJobRepository(JeeSiteDbContext db) => _db = db;
    public IQueryable<SysJob> Query() => _db.Set<SysJob>().AsNoTracking();
    public async Task<SysJob?> GetAsync(object id) => await _db.Set<SysJob>().FindAsync(id);
    public async Task<List<SysJob>> FindListAsync() => await _db.Set<SysJob>().AsNoTracking().ToListAsync();
    public async Task AddAsync(SysJob entity) { _db.Set<SysJob>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(SysJob entity) { _db.Set<SysJob>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(SysJob entity) { _db.Set<SysJob>().Remove(entity); await _db.SaveChangesAsync(); }
    public async Task<List<SysJob>> FindRunningJobsAsync() => await _db.Set<SysJob>().Where(j => j.RunStatus == "1" && j.Status == "0").AsNoTracking().ToListAsync();
}
