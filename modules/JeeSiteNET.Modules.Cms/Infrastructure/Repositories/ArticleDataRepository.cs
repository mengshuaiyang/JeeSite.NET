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

// 定义class ArticleDataRepository
// 定义类：ArticleDataRepository
public class ArticleDataRepository : IArticleDataRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 ArticleDataRepository
    // 构造函数：ArticleDataRepository
    public ArticleDataRepository(JeeSiteDbContext db) => _db = db;

    // 方法：Query
    public IQueryable<ArticleData> Query() => _db.Set<ArticleData>().AsNoTracking();
    // 方法：GetAsync
    public async Task<ArticleData?> GetAsync(object id) => await _db.Set<ArticleData>().FindAsync(id);
    // 方法：FindListAsync
    public async Task<List<ArticleData>> FindListAsync() => await _db.Set<ArticleData>().AsNoTracking().ToListAsync();
    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(ArticleData entity) { _db.Set<ArticleData>().Add(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(ArticleData entity) { _db.Set<ArticleData>().Update(entity); await _db.SaveChangesAsync(); }
    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(ArticleData entity) { _db.Set<ArticleData>().Remove(entity); await _db.SaveChangesAsync(); }
}
