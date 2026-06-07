using JeeSiteNET.Modules.Cms.Domain.Entities;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Cms.Infrastructure.Repositories;

public class ArticleTagRepository : IArticleTagRepository
{
    private readonly JeeSiteDbContext _db;
    public ArticleTagRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<ArticleTag> Query() => _db.Set<ArticleTag>().AsNoTracking();
    public async Task<ArticleTag?> GetAsync(object id) => await _db.Set<ArticleTag>().FindAsync(id);
    public async Task<List<ArticleTag>> FindListAsync() => await _db.Set<ArticleTag>().AsNoTracking().ToListAsync();
    public async Task AddAsync(ArticleTag entity) { _db.Set<ArticleTag>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(ArticleTag entity) { _db.Set<ArticleTag>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(ArticleTag entity) { _db.Set<ArticleTag>().Remove(entity); await _db.SaveChangesAsync(); }
}
