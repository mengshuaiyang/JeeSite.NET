using JeeSiteNET.Modules.Cms.Domain.Entities;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Cms.Infrastructure.Repositories;

public class ArticlePosIdRepository : IArticlePosIdRepository
{
    private readonly JeeSiteDbContext _db;
    public ArticlePosIdRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<ArticlePosId> Query() => _db.Set<ArticlePosId>().AsNoTracking();
    public async Task<ArticlePosId?> GetAsync(object id) => await _db.Set<ArticlePosId>().FindAsync(id);
    public async Task<List<ArticlePosId>> FindListAsync() => await _db.Set<ArticlePosId>().AsNoTracking().ToListAsync();
    public async Task AddAsync(ArticlePosId entity) { _db.Set<ArticlePosId>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(ArticlePosId entity) { _db.Set<ArticlePosId>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(ArticlePosId entity) { _db.Set<ArticlePosId>().Remove(entity); await _db.SaveChangesAsync(); }
}
