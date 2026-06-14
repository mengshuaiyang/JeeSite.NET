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

// 定义class MsgPushedRepository
// 定义类：MsgPushedRepository
public class MsgPushedRepository : IMsgPushedRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 MsgPushedRepository
    // 构造函数：MsgPushedRepository
    public MsgPushedRepository(JeeSiteDbContext db) => _db = db;

    // 方法：Query
    public IQueryable<MsgPushed> Query() => _db.Set<MsgPushed>().AsNoTracking();

    // 方法：GetAsync
    public async Task<MsgPushed?> GetAsync(string id) => await _db.Set<MsgPushed>().FindAsync(id);

    // 方法：AddAsync
    public async Task AddAsync(MsgPushed entity) => await _db.Set<MsgPushed>().AddAsync(entity);

    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public Task UpdateAsync(MsgPushed entity) { _db.Set<MsgPushed>().Update(entity); return Task.CompletedTask; }

    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(string id)
    {
        var entity = await _db.Set<MsgPushed>().FindAsync(id);
        // if 条件判断
        if (entity != null) _db.Set<MsgPushed>().Remove(entity);
    }

    // 方法：SaveChangesAsync
    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
