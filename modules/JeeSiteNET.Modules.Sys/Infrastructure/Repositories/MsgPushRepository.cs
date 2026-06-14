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

// 定义class MsgPushRepository
// 定义类：MsgPushRepository
public class MsgPushRepository : IMsgPushRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 MsgPushRepository
    // 构造函数：MsgPushRepository
    public MsgPushRepository(JeeSiteDbContext db) => _db = db;

    // 方法：Query
    public IQueryable<MsgPush> Query() => _db.Set<MsgPush>().AsNoTracking();

    // 方法：GetAsync
    public async Task<MsgPush?> GetAsync(string id) => await _db.Set<MsgPush>().FindAsync(id);

    // 方法：AddAsync
    public async Task AddAsync(MsgPush entity) => await _db.Set<MsgPush>().AddAsync(entity);

    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public Task UpdateAsync(MsgPush entity) { _db.Set<MsgPush>().Update(entity); return Task.CompletedTask; }

    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(string id)
    {
        var entity = await _db.Set<MsgPush>().FindAsync(id);
        // if 条件判断
        if (entity != null) _db.Set<MsgPush>().Remove(entity);
    }

    // 方法：SaveChangesAsync
    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
