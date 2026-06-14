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

// 定义class SiteRepository
// 定义类：SiteRepository
public class SiteRepository : ISiteRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 SiteRepository
    // 构造函数：SiteRepository
    public SiteRepository(JeeSiteDbContext db) => _db = db;
    // 方法：Query
    public IQueryable<Site> Query() => _db.Set<Site>().AsNoTracking();
    // 方法：GetAsync
    public async Task<Site?> GetAsync(object id) => await _db.Set<Site>().FindAsync(id);
    // 方法：FindListAsync
    public async Task<List<Site>> FindListAsync() => await _db.Set<Site>().AsNoTracking().ToListAsync();
    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(Site entity) { _db.Set<Site>().Add(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(Site entity) { _db.Set<Site>().Update(entity); await _db.SaveChangesAsync(); }
    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(Site entity) { _db.Set<Site>().Remove(entity); await _db.SaveChangesAsync(); }
}
