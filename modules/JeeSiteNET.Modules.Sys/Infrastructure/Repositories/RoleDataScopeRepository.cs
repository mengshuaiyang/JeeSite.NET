    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.Sys.Infrastructure.Repositories 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Infrastructure.Repositories
namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

// 定义class RoleDataScopeRepository
// 定义类：RoleDataScopeRepository
public class RoleDataScopeRepository : IRoleDataScopeRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;

    // 方法 RoleDataScopeRepository
    // 构造函数：RoleDataScopeRepository
    public RoleDataScopeRepository(JeeSiteDbContext db)
    {
        _db = db;
    }

    // 方法：Query
    public IQueryable<RoleDataScope> Query() => _db.Set<RoleDataScope>().AsNoTracking();

    // 方法 GetAsync
    // 方法：GetAsync
    public async Task<RoleDataScope?> GetAsync(string roleCode, string menuCode)
    {
        // return 返回结果
        return await _db.Set<RoleDataScope>()
            // 数据库操作：异步取首条或默认值
            .FirstOrDefaultAsync(e => e.RoleCode == roleCode && e.MenuCode == menuCode);
    }

    // 方法：FindListAsync
    public async Task<List<RoleDataScope>> FindListAsync() => await _db.Set<RoleDataScope>().ToListAsync();

    // 方法 FindByRoleAsync
    // 方法：FindByRoleAsync
    public async Task<List<RoleDataScope>> FindByRoleAsync(string roleCode)
    {
        // return 返回结果
        return await _db.Set<RoleDataScope>()
            // 数据库操作：条件过滤
            .Where(e => e.RoleCode == roleCode)
            // 数据库操作：异步查询为列表
            .ToListAsync();
    }

    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(RoleDataScope entity)
    {
        // await 异步等待
        await _db.Set<RoleDataScope>().AddAsync(entity);
    }

    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(RoleDataScope entity)
    {
        // 调用 Update
        _db.Set<RoleDataScope>().Update(entity);
    }

    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(string roleCode, string menuCode)
    {
        var entity = await _db.Set<RoleDataScope>()
            // 数据库操作：异步取首条或默认值
            .FirstOrDefaultAsync(e => e.RoleCode == roleCode && e.MenuCode == menuCode);
        // if 条件判断
        if (entity != null)
        {
            // 集合操作：移除元素
            _db.Set<RoleDataScope>().Remove(entity);
        }
    }

    // 方法 DeleteByRoleAsync
    // 方法：DeleteByRoleAsync
    public async Task DeleteByRoleAsync(string roleCode)
    {
        var entities = await _db.Set<RoleDataScope>()
            // 数据库操作：条件过滤
            .Where(e => e.RoleCode == roleCode)
            // 数据库操作：异步查询为列表
            .ToListAsync();
        // 集合操作：批量移除
        _db.Set<RoleDataScope>().RemoveRange(entities);
    }

    // 方法：SaveChangesAsync
    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
