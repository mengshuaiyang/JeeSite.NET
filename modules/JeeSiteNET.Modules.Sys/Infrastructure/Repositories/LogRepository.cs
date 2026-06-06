using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class LogRepository : ILogRepository
{
    private readonly JeeSiteDbContext _db;
    public LogRepository(JeeSiteDbContext db) => _db = db;
    public IQueryable<Log> Query() => _db.Set<Log>().AsNoTracking();
    public async Task<Log?> GetAsync(object id) => await _db.Set<Log>().FindAsync(id);
    public async Task<List<Log>> FindListAsync() => await _db.Set<Log>().AsNoTracking().ToListAsync();
    public async Task AddAsync(Log entity) { await _db.Set<Log>().AddAsync(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(Log entity) { _db.Set<Log>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(Log entity) { _db.Set<Log>().Remove(entity); await _db.SaveChangesAsync(); }
}
