using JeeSiteNET.Core;
using JeeSiteNET.Modules.Tasks.Domain.Entities;
using JeeSiteNET.Modules.Tasks.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Tasks.Infrastructure.Repositories;

public class JobLogRepository : IJobLogRepository
{
    private readonly JeeSiteDbContext _db;
    public JobLogRepository(JeeSiteDbContext db) => _db = db;
    public IQueryable<JobLog> Query() => _db.Set<JobLog>().AsNoTracking();
    public async Task<JobLog?> GetAsync(object id) => await _db.Set<JobLog>().FindAsync(id);
    public async Task<List<JobLog>> FindListAsync() => await _db.Set<JobLog>().AsNoTracking().ToListAsync();
    public async Task AddAsync(JobLog entity) { _db.Set<JobLog>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(JobLog entity) { _db.Set<JobLog>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(JobLog entity) { _db.Set<JobLog>().Remove(entity); await _db.SaveChangesAsync(); }
    public async Task<List<JobLog>> FindByJobIdAsync(string jobId, int limit = 50)
        => await _db.Set<JobLog>().Where(l => l.JobId == jobId).OrderByDescending(l => l.RunDate).Take(limit).AsNoTracking().ToListAsync();
}
