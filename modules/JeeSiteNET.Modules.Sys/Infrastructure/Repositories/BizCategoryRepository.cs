    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.Sys.Infrastructure.Repositories 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Infrastructure.Repositories
namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

// 定义class BizCategoryRepository
// 定义类：BizCategoryRepository
public class BizCategoryRepository : IBizCategoryRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 BizCategoryRepository
    // 构造函数：BizCategoryRepository
    public BizCategoryRepository(JeeSiteDbContext db) => _db = db;

    // 方法：Query
    public IQueryable<BizCategory> Query() => _db.Set<BizCategory>().AsNoTracking();
    // 方法：GetAsync
    public async Task<BizCategory?> GetAsync(object id) => await _db.Set<BizCategory>().FindAsync(id);
    // 方法：FindListAsync
    public async Task<List<BizCategory>> FindListAsync() => await _db.Set<BizCategory>().AsNoTracking().ToListAsync();
    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(BizCategory entity) { _db.Set<BizCategory>().Add(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(BizCategory entity) { _db.Set<BizCategory>().Update(entity); await _db.SaveChangesAsync(); }
    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(BizCategory entity) { _db.Set<BizCategory>().Remove(entity); await _db.SaveChangesAsync(); }
}
