    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
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

// 定义class MenuRepository
// 定义类：MenuRepository
public class MenuRepository : IMenuRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;

    // 构造函数 MenuRepository
    // 构造函数：MenuRepository
    public MenuRepository(JeeSiteDbContext db) => _db = db;

    // 方法：Query
    public IQueryable<Menu> Query() => _db.Set<Menu>().AsNoTracking();

    // 方法 GetAsync
    // 方法：GetAsync
    public async Task<Menu?> GetAsync(object id)
        => await _db.Set<Menu>().FindAsync(id);

    // 方法 FindListAsync
    // 方法：FindListAsync
    public async Task<List<Menu>> FindListAsync()
        // 数据库操作：不跟踪查询（只读）
        => await _db.Set<Menu>().AsNoTracking().ToListAsync();

    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(Menu entity)
    {
        // await 异步等待
        await _db.Set<Menu>().AddAsync(entity);
        // await 异步等待
        await _db.SaveChangesAsync();
    }

    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(Menu entity)
    {
        // 调用 Update
        _db.Set<Menu>().Update(entity);
        // await 异步等待
        await _db.SaveChangesAsync();
    }

    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(Menu entity)
    {
        // 集合操作：移除元素
        _db.Set<Menu>().Remove(entity);
        // await 异步等待
        await _db.SaveChangesAsync();
    }

    // 方法 GetPermissionsByRoleCodesAsync
    // 方法：GetPermissionsByRoleCodesAsync
    public async Task<List<string>> GetPermissionsByRoleCodesAsync(List<string> roleCodes)
    {
        var menuCodes = await _db.Set<RoleMenu>()
            // 集合操作：检查是否包含
            .Where(rm => roleCodes.Contains(rm.RoleCode))
            // 数据库操作：投影选择
            .Select(rm => rm.MenuCode)
            // 数据库操作：异步查询为列表
            .ToListAsync();

        // return 返回结果
        return await _db.Set<Menu>()
            // 集合操作：检查是否包含
            .Where(m => menuCodes.Contains(m.MenuCode) && m.Status == "0" && m.Permission != null)
            // 数据库操作：投影选择
            .Select(m => m.Permission!)
            // 数据库操作：异步查询为列表
            .ToListAsync();
    }
}
