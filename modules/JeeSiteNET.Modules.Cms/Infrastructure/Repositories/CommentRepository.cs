using JeeSiteNET.Core;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Cms.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly JeeSiteDbContext _db;
    public CommentRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<Comment> Query() => _db.Set<Comment>().AsNoTracking();
    public async Task<Comment?> GetAsync(object id) => await _db.Set<Comment>().FindAsync(id);
    public async Task<List<Comment>> FindListAsync() => await _db.Set<Comment>().AsNoTracking().ToListAsync();
    public async Task AddAsync(Comment entity) { _db.Set<Comment>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(Comment entity) { _db.Set<Comment>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(Comment entity) { _db.Set<Comment>().Remove(entity); await _db.SaveChangesAsync(); }

    public async Task<PageResult<Comment>> FindPageAsync(PageRequest<Comment> request)
    {
        var query = _db.Set<Comment>().AsNoTracking()
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.ArticleCode), e => e.ArticleCode == request.Entity!.ArticleCode)
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Status), e => e.Status == request.Entity!.Status)
            .OrderByDescending(e => e.CreateDate);
        var total = await query.CountAsync();
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        return new PageResult<Comment> { List = list, Total = total, PageNo = request.PageNo, PageSize = request.PageSize };
    }
}
