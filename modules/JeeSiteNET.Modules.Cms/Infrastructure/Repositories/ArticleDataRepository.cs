using JeeSiteNET.Modules.Cms.Domain.Entities;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Cms.Infrastructure.Repositories;

public class ArticleDataRepository : IArticleDataRepository
{
    private readonly JeeSiteDbContext _db;
    public ArticleDataRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<ArticleData> Query() => _db.Set<ArticleData>().AsNoTracking();
    public async Task<ArticleData?> GetAsync(object id) => await _db.Set<ArticleData>().FindAsync(id);
    public async Task<List<ArticleData>> FindListAsync() => await _db.Set<ArticleData>().AsNoTracking().ToListAsync();
    public async Task AddAsync(ArticleData entity) { _db.Set<ArticleData>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(ArticleData entity) { _db.Set<ArticleData>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(ArticleData entity) { _db.Set<ArticleData>().Remove(entity); await _db.SaveChangesAsync(); }
}
