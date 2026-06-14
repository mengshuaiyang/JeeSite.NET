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

// 定义class UserDataScopeRepository
// 定义类：UserDataScopeRepository
public class UserDataScopeRepository : IUserDataScopeRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;

    // 方法 UserDataScopeRepository
    // 构造函数：UserDataScopeRepository
    public UserDataScopeRepository(JeeSiteDbContext db)
    {
        _db = db;
    }

    // 方法：Query
    public IQueryable<UserDataScope> Query() => _db.Set<UserDataScope>().AsNoTracking();

    // 方法 GetByUserAsync
    // 方法：GetByUserAsync
    public async Task<UserDataScope?> GetByUserAsync(string userCode)
    {
        // return 返回结果
        return await _db.Set<UserDataScope>()
            // 数据库操作：异步取首条或默认值
            .FirstOrDefaultAsync(e => e.UserCode == userCode);
    }

    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(UserDataScope entity)
    {
        // await 异步等待
        await _db.Set<UserDataScope>().AddAsync(entity);
    }

    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(UserDataScope entity)
    {
        // 调用 Update
        _db.Set<UserDataScope>().Update(entity);
    }

    // 方法：SaveChangesAsync
    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
