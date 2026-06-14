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

// 定义class UserRoleRepository
// 定义类：UserRoleRepository
public class UserRoleRepository : IUserRoleRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;

    // 构造函数 UserRoleRepository
    // 构造函数：UserRoleRepository
    public UserRoleRepository(JeeSiteDbContext db) => _db = db;

    // 方法 GetRoleCodesByUserAsync
    // 方法：GetRoleCodesByUserAsync
    public async Task<List<string>> GetRoleCodesByUserAsync(string userCode)
        => await _db.Set<UserRole>()
            // 数据库操作：条件过滤
            .Where(ur => ur.UserCode == userCode)
            // 数据库操作：投影选择
            .Select(ur => ur.RoleCode)
            // 数据库操作：异步查询为列表
            .ToListAsync();

    // 方法 GetUserCodesByRoleAsync
    // 方法：GetUserCodesByRoleAsync
    public async Task<List<string>> GetUserCodesByRoleAsync(string roleCode)
        => await _db.Set<UserRole>()
            // 数据库操作：条件过滤
            .Where(ur => ur.RoleCode == roleCode)
            // 数据库操作：投影选择
            .Select(ur => ur.UserCode)
            // 数据库操作：异步查询为列表
            .ToListAsync();

    // 方法 SaveRoleUsersAsync
    // 方法：SaveRoleUsersAsync
    public async Task SaveRoleUsersAsync(string roleCode, List<string> userCodes)
    {
        var existing = await _db.Set<UserRole>()
            // 数据库操作：条件过滤
            .Where(ur => ur.RoleCode == roleCode)
            // 数据库操作：异步查询为列表
            .ToListAsync();

        // 集合操作：批量移除
        _db.Set<UserRole>().RemoveRange(existing);

        // foreach 遍历集合
        foreach (var userCode in userCodes)
        {
            // 集合操作：添加元素
            _db.Set<UserRole>().Add(new UserRole
            {
                UserCode = userCode,
                RoleCode = roleCode
            });
        }

        // await 异步等待
        await _db.SaveChangesAsync();
    }

    // 方法 SaveUserRolesAsync
    // 方法：SaveUserRolesAsync
    public async Task SaveUserRolesAsync(string userCode, List<string> roleCodes)
    {
        var existing = await _db.Set<UserRole>()
            // 数据库操作：条件过滤
            .Where(ur => ur.UserCode == userCode)
            // 数据库操作：异步查询为列表
            .ToListAsync();

        // 集合操作：批量移除
        _db.Set<UserRole>().RemoveRange(existing);

        // foreach 遍历集合
        foreach (var roleCode in roleCodes)
        {
            // 集合操作：添加元素
            _db.Set<UserRole>().Add(new UserRole
            {
                UserCode = userCode,
                RoleCode = roleCode
            });
        }

        // await 异步等待
        await _db.SaveChangesAsync();
    }

    // 方法 DeleteByUserAsync
    // 方法：DeleteByUserAsync
    public async Task DeleteByUserAsync(string userCode)
    {
        var existing = await _db.Set<UserRole>()
            // 数据库操作：条件过滤
            .Where(ur => ur.UserCode == userCode)
            // 数据库操作：异步查询为列表
            .ToListAsync();

        // 集合操作：批量移除
        _db.Set<UserRole>().RemoveRange(existing);
        // await 异步等待
        await _db.SaveChangesAsync();
    }
}
