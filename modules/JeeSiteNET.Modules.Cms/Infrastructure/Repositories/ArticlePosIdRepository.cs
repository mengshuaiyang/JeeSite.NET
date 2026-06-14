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

// 定义class ArticlePosIdRepository
// 定义类：ArticlePosIdRepository
public class ArticlePosIdRepository : IArticlePosIdRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 ArticlePosIdRepository
    // 构造函数：ArticlePosIdRepository
    public ArticlePosIdRepository(JeeSiteDbContext db) => _db = db;

    // 方法：Query
    public IQueryable<ArticlePosId> Query() => _db.Set<ArticlePosId>().AsNoTracking();
    // 方法：GetAsync
    public async Task<ArticlePosId?> GetAsync(object id) => await _db.Set<ArticlePosId>().FindAsync(id);
    // 方法：FindListAsync
    public async Task<List<ArticlePosId>> FindListAsync() => await _db.Set<ArticlePosId>().AsNoTracking().ToListAsync();
    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(ArticlePosId entity) { _db.Set<ArticlePosId>().Add(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(ArticlePosId entity) { _db.Set<ArticlePosId>().Update(entity); await _db.SaveChangesAsync(); }
    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(ArticlePosId entity) { _db.Set<ArticlePosId>().Remove(entity); await _db.SaveChangesAsync(); }
}
