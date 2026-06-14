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

// 定义class EmpUserRepository
// 定义类：EmpUserRepository
public class EmpUserRepository : IEmpUserRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 EmpUserRepository
    // 构造函数：EmpUserRepository
    public EmpUserRepository(JeeSiteDbContext db) => _db = db;

    // 方法：Query
    public IQueryable<EmpUser> Query() => _db.Set<EmpUser>().AsNoTracking();

    // 方法 GetAsync
    // 方法：GetAsync
    public async Task<EmpUser?> GetAsync(string empCode, string userCode)
        => await _db.Set<EmpUser>().FindAsync(empCode, userCode);

    // 方法 GetByEmpCodeAsync
    // 方法：GetByEmpCodeAsync
    public async Task<List<EmpUser>> GetByEmpCodeAsync(string empCode)
        // 数据库操作：条件过滤
        => await _db.Set<EmpUser>().Where(e => e.EmpCode == empCode).ToListAsync();

    // 方法 GetByUserCodeAsync
    // 方法：GetByUserCodeAsync
    public async Task<List<EmpUser>> GetByUserCodeAsync(string userCode)
        // 数据库操作：条件过滤
        => await _db.Set<EmpUser>().Where(e => e.UserCode == userCode).ToListAsync();

    // 方法 GetUserCodesByEmpCodeAsync
    // 方法：GetUserCodesByEmpCodeAsync
    public async Task<List<string>> GetUserCodesByEmpCodeAsync(string empCode)
        // 数据库操作：条件过滤
        => await _db.Set<EmpUser>().Where(e => e.EmpCode == empCode).Select(e => e.UserCode).ToListAsync();

    // 方法 GetEmpCodesByUserCodeAsync
    // 方法：GetEmpCodesByUserCodeAsync
    public async Task<List<string>> GetEmpCodesByUserCodeAsync(string userCode)
        // 数据库操作：条件过滤
        => await _db.Set<EmpUser>().Where(e => e.UserCode == userCode).Select(e => e.EmpCode).ToListAsync();

    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(EmpUser entity)
    {
        // 集合操作：添加元素
        _db.Set<EmpUser>().Add(entity);
        // await 异步等待
        await _db.SaveChangesAsync();
    }

    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(string empCode, string userCode)
    {
        var entity = await _db.Set<EmpUser>().FindAsync(empCode, userCode);
        // if 条件判断
        if (entity != null)
        {
            // 集合操作：移除元素
            _db.Set<EmpUser>().Remove(entity);
            // await 异步等待
            await _db.SaveChangesAsync();
        }
    }

    // 方法 DeleteByEmpCodeAsync
    // 方法：DeleteByEmpCodeAsync
    public async Task DeleteByEmpCodeAsync(string empCode)
    {
        // 数据库操作：条件过滤
        var entities = await _db.Set<EmpUser>().Where(e => e.EmpCode == empCode).ToListAsync();
        // 集合操作：批量移除
        _db.Set<EmpUser>().RemoveRange(entities);
        // await 异步等待
        await _db.SaveChangesAsync();
    }
}
