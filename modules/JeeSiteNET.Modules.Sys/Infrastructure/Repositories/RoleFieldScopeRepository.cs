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

// 定义class RoleFieldScopeRepository
// 定义类：RoleFieldScopeRepository
public class RoleFieldScopeRepository : IRoleFieldScopeRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;

    // 方法 RoleFieldScopeRepository
    // 构造函数：RoleFieldScopeRepository
    public RoleFieldScopeRepository(JeeSiteDbContext db)
    {
        _db = db;
    }

    // 方法：Query
    public IQueryable<RoleFieldScope> Query() => _db.Set<RoleFieldScope>().AsNoTracking();

    // 方法 GetAsync
    // 方法：GetAsync
    public async Task<RoleFieldScope?> GetAsync(string id)
        => await _db.Set<RoleFieldScope>().FindAsync(id);

    // 方法 GetByRoleMenuAsync
    // 方法：GetByRoleMenuAsync
    public async Task<List<RoleFieldScope>> GetByRoleMenuAsync(string roleCode, string menuCode)
        => await _db.Set<RoleFieldScope>()
            // 数据库操作：条件过滤
            .Where(e => e.RoleCode == roleCode && e.MenuCode == menuCode)
            // 数据库操作：异步查询为列表
            .ToListAsync();

    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(RoleFieldScope entity)
        // 数据库操作：异步添加
        => await _db.Set<RoleFieldScope>().AddAsync(entity);

    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public Task UpdateAsync(RoleFieldScope entity)
    {
        // 调用 Update
        _db.Set<RoleFieldScope>().Update(entity);
        // return 返回结果
        return Task.CompletedTask;
    }

    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(string id)
    {
        var entity = await _db.Set<RoleFieldScope>().FindAsync(id);
        // if 条件判断
        if (entity != null) _db.Set<RoleFieldScope>().Remove(entity);
    }

    // 方法：SaveChangesAsync
    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
