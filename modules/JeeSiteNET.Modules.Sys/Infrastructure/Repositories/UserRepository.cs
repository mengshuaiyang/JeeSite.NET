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

// 定义class UserRepository
// 定义类：UserRepository
public class UserRepository : IUserRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;

    // 构造函数 UserRepository
    // 构造函数：UserRepository
    public UserRepository(JeeSiteDbContext db) => _db = db;

    // 方法：Query
    public IQueryable<User> Query() => _db.Set<User>().AsNoTracking();

    // 方法 GetAsync
    // 方法：GetAsync
    public async Task<User?> GetAsync(object id)
        => await _db.Set<User>().FindAsync(id);

    // 方法 FindListAsync
    // 方法：FindListAsync
    public async Task<List<User>> FindListAsync()
        // 数据库操作：不跟踪查询（只读）
        => await _db.Set<User>().AsNoTracking().ToListAsync();

    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(User entity)
    {
        // await 异步等待
        await _db.Set<User>().AddAsync(entity);
        // await 异步等待
        await _db.SaveChangesAsync();
    }

    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(User entity)
    {
        // 调用 Update
        _db.Set<User>().Update(entity);
        // await 异步等待
        await _db.SaveChangesAsync();
    }

    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(User entity)
    {
        // 集合操作：移除元素
        _db.Set<User>().Remove(entity);
        // await 异步等待
        await _db.SaveChangesAsync();
    }

    // 方法 GetByLoginCodeAsync
    // 方法：GetByLoginCodeAsync
    public async Task<User?> GetByLoginCodeAsync(string loginCode)
        // 数据库操作：不跟踪查询（只读）
        => await _db.Set<User>().AsNoTracking()
            // 数据库操作：异步取首条或默认值
            .FirstOrDefaultAsync(u => u.LoginCode == loginCode);

    // 方法 GetByPhoneAsync
    // 方法：GetByPhoneAsync
    public async Task<User?> GetByPhoneAsync(string phone)
        // 数据库操作：不跟踪查询（只读）
        => await _db.Set<User>().AsNoTracking()
            // 数据库操作：异步取首条或默认值
            .FirstOrDefaultAsync(u => u.Phone != null && u.Phone == phone);

    // 方法 GetByEmailAsync
    // 方法：GetByEmailAsync
    public async Task<User?> GetByEmailAsync(string email)
        // 数据库操作：不跟踪查询（只读）
        => await _db.Set<User>().AsNoTracking()
            // 数据库操作：异步取首条或默认值
            .FirstOrDefaultAsync(u => u.Email != null && u.Email == email);
}
