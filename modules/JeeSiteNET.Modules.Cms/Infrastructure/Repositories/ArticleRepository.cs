using JeeSiteNET.Core;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Cms.Infrastructure.Repositories;

public class ArticleRepository : IArticleRepository
{
    private readonly JeeSiteDbContext _db;
    public ArticleRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<Article> Query() => _db.Set<Article>().AsNoTracking();
    public async Task<Article?> GetAsync(object id) => await _db.Set<Article>().FindAsync(id);
    public async Task<Article?> GetWithDetailAsync(string articleCode) => await _db.Set<Article>()
        .Include(e => e.ArticleData)
        .Include(e => e.PosIds)
        .Include(e => e.ArticleTags)
        .AsSplitQuery()
        .FirstOrDefaultAsync(e => e.ArticleCode == articleCode);
    public async Task<List<Article>> FindListAsync() => await _db.Set<Article>().AsNoTracking().ToListAsync();
    public async Task AddAsync(Article entity) { _db.Set<Article>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(Article entity) { _db.Set<Article>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(Article entity) { _db.Set<Article>().Remove(entity); await _db.SaveChangesAsync(); }

    public async Task<PageResult<Article>> FindPageAsync(PageRequest<Article> request)
    {
        var query = _db.Set<Article>().AsNoTracking()
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Title), e => e.Title.Contains(request.Entity!.Title!))
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.CategoryCode), e => e.CategoryCode == request.Entity!.CategoryCode)
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Tags), e => e.Tags!.Contains(request.Entity!.Tags!))
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.IsTop), e => e.IsTop == request.Entity!.IsTop)
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.IsRecommend), e => e.IsRecommend == request.Entity!.IsRecommend)
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.IsHot), e => e.IsHot == request.Entity!.IsHot)
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Status), e => e.Status == request.Entity!.Status)
            .OrderByDescending(e => e.IsTop).ThenByDescending(e => e.PublishDate);
        var total = await query.CountAsync();
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        return new PageResult<Article> { List = list, Total = total, PageNo = request.PageNo, PageSize = request.PageSize };
    }
}
