using JeeSiteNET.Modules.App.Domain.Entities;
using JeeSiteNET.Modules.App.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.App.Infrastructure.Repositories;

public class AppCommentRepository : IAppCommentRepository
{
    private readonly JeeSiteDbContext _db;
    public AppCommentRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<AppComment> Query() => _db.Set<AppComment>().AsNoTracking();
    public async Task<AppComment?> GetAsync(object id) => await _db.Set<AppComment>().FindAsync(id);
    public async Task<List<AppComment>> FindListAsync() => await _db.Set<AppComment>().AsNoTracking().ToListAsync();
    public async Task AddAsync(AppComment entity) { _db.Set<AppComment>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(AppComment entity) { _db.Set<AppComment>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(AppComment entity) { _db.Set<AppComment>().Remove(entity); await _db.SaveChangesAsync(); }
}
