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

// 定义class RoleMenuRepository
// 定义类：RoleMenuRepository
public class RoleMenuRepository : IRoleMenuRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 RoleMenuRepository
    // 构造函数：RoleMenuRepository
    public RoleMenuRepository(JeeSiteDbContext db) => _db = db;

    // 方法 GetMenuCodesByRoleAsync
    // 方法：GetMenuCodesByRoleAsync
    public async Task<List<string>> GetMenuCodesByRoleAsync(string roleCode)
        // 数据库操作：条件过滤
        => await _db.Set<RoleMenu>().Where(rm => rm.RoleCode == roleCode).Select(rm => rm.MenuCode).ToListAsync();

    // 方法 SaveRoleMenusAsync
    // 方法：SaveRoleMenusAsync
    public async Task SaveRoleMenusAsync(string roleCode, List<string> menuCodes)
    {
        // 数据库操作：条件过滤
        var existing = await _db.Set<RoleMenu>().Where(rm => rm.RoleCode == roleCode).ToListAsync();
        // 集合操作：批量移除
        _db.Set<RoleMenu>().RemoveRange(existing);
        // foreach 遍历集合
        foreach (var menuCode in menuCodes)
            // 集合操作：添加元素
            _db.Set<RoleMenu>().Add(new RoleMenu { RoleCode = roleCode, MenuCode = menuCode });
        // await 异步等待
        await _db.SaveChangesAsync();
    }

    // 方法 GetMenuCodesByRolesAsync
    // 方法：GetMenuCodesByRolesAsync
    public async Task<List<string>> GetMenuCodesByRolesAsync(List<string> roleCodes)
        // 集合操作：检查是否包含
        => await _db.Set<RoleMenu>().Where(rm => roleCodes.Contains(rm.RoleCode)).Select(rm => rm.MenuCode).ToListAsync();
}
