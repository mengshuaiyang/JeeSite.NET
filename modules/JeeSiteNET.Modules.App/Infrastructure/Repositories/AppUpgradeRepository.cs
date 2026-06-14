    // 引入 JeeSiteNET.Modules.App.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.App.Domain.Entities
using JeeSiteNET.Modules.App.Domain.Entities;
    // 引入 JeeSiteNET.Modules.App.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.App.Domain.Interfaces
using JeeSiteNET.Modules.App.Domain.Interfaces;
    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.App.Infrastructure.Repositories 命名空间
// 定义命名空间：JeeSiteNET.Modules.App.Infrastructure.Repositories
namespace JeeSiteNET.Modules.App.Infrastructure.Repositories;

// 定义class AppUpgradeRepository
// 定义类：AppUpgradeRepository
public class AppUpgradeRepository : IAppUpgradeRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 AppUpgradeRepository
    // 构造函数：AppUpgradeRepository
    public AppUpgradeRepository(JeeSiteDbContext db) => _db = db;

    // 方法：Query
    public IQueryable<AppUpgrade> Query() => _db.Set<AppUpgrade>().AsNoTracking();
    // 方法：GetAsync
    public async Task<AppUpgrade?> GetAsync(object id) => await _db.Set<AppUpgrade>().FindAsync(id);
    // 方法：FindListAsync
    public async Task<List<AppUpgrade>> FindListAsync() => await _db.Set<AppUpgrade>().AsNoTracking().ToListAsync();
    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(AppUpgrade entity) { _db.Set<AppUpgrade>().Add(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(AppUpgrade entity) { _db.Set<AppUpgrade>().Update(entity); await _db.SaveChangesAsync(); }
    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(AppUpgrade entity) { _db.Set<AppUpgrade>().Remove(entity); await _db.SaveChangesAsync(); }
}
