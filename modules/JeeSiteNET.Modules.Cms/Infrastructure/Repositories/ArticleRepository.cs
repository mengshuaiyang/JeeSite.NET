    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
using JeeSiteNET.Modules.Cms.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Cms.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Interfaces
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.Cms.Infrastructure.Repositories 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Infrastructure.Repositories
namespace JeeSiteNET.Modules.Cms.Infrastructure.Repositories;

// 定义class ArticleRepository
// 定义类：ArticleRepository
public class ArticleRepository : IArticleRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 ArticleRepository
    // 构造函数：ArticleRepository
    public ArticleRepository(JeeSiteDbContext db) => _db = db;

    // 方法：Query
    public IQueryable<Article> Query() => _db.Set<Article>().AsNoTracking();
    // 方法：GetAsync
    public async Task<Article?> GetAsync(object id) => await _db.Set<Article>().FindAsync(id);
    // 方法 GetWithDetailAsync
    // 方法：GetWithDetailAsync
    public async Task<Article?> GetWithDetailAsync(string articleCode) => await _db.Set<Article>()
        // 数据库操作：加载关联实体
        .Include(e => e.ArticleData)
        // 数据库操作：加载关联实体
        .Include(e => e.PosIds)
        // 数据库操作：加载关联实体
        .Include(e => e.ArticleTags)
        .AsSplitQuery()
        // 数据库操作：异步取首条或默认值
        .FirstOrDefaultAsync(e => e.ArticleCode == articleCode);
    // 方法：FindListAsync
    public async Task<List<Article>> FindListAsync() => await _db.Set<Article>().AsNoTracking().ToListAsync();
    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(Article entity) { _db.Set<Article>().Add(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(Article entity) { _db.Set<Article>().Update(entity); await _db.SaveChangesAsync(); }
    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(Article entity) { _db.Set<Article>().Remove(entity); await _db.SaveChangesAsync(); }

    // 方法 FindPageAsync
    // 方法：FindPageAsync
    public async Task<PageResult<Article>> FindPageAsync(PageRequest<Article> request)
    {
        // 数据库操作：不跟踪查询（只读）
        var query = _db.Set<Article>().AsNoTracking()
            // 集合操作：检查是否包含
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Title), e => e.Title.Contains(request.Entity!.Title!))
            // 调用 IsNullOrEmpty
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.CategoryCode), e => e.CategoryCode == request.Entity!.CategoryCode)
            // 集合操作：检查是否包含
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Tags), e => e.Tags!.Contains(request.Entity!.Tags!))
            // 调用 IsNullOrEmpty
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.IsTop), e => e.IsTop == request.Entity!.IsTop)
            // 调用 IsNullOrEmpty
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.IsRecommend), e => e.IsRecommend == request.Entity!.IsRecommend)
            // 调用 IsNullOrEmpty
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.IsHot), e => e.IsHot == request.Entity!.IsHot)
            // 调用 IsNullOrEmpty
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Status), e => e.Status == request.Entity!.Status)
            // 数据库操作：降序排序
            .OrderByDescending(e => e.IsTop).ThenByDescending(e => e.PublishDate);
        // 数据库操作：异步统计数量
        var total = await query.CountAsync();
        // 数据库操作：异步查询为列表
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        // return 返回结果
        return new PageResult<Article> { List = list, Total = total, PageNo = request.PageNo, PageSize = request.PageSize };
    }
}
