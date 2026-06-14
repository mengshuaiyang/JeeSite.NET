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

// 定义class DictTypeRepository
// 定义类：DictTypeRepository
public class DictTypeRepository : IDictTypeRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;

    // 构造函数 DictTypeRepository
    // 构造函数：DictTypeRepository
    public DictTypeRepository(JeeSiteDbContext db) => _db = db;

    // 方法：Query
    public IQueryable<DictType> Query() => _db.Set<DictType>().AsNoTracking();

    // 方法 GetAsync
    // 方法：GetAsync
    public async Task<DictType?> GetAsync(object id)
        => await _db.Set<DictType>().FindAsync(id);

    // 方法 FindListAsync
    // 方法：FindListAsync
    public async Task<List<DictType>> FindListAsync()
        // 数据库操作：不跟踪查询（只读）
        => await _db.Set<DictType>().AsNoTracking().ToListAsync();

    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(DictType entity)
    {
        // await 异步等待
        await _db.Set<DictType>().AddAsync(entity);
        // await 异步等待
        await _db.SaveChangesAsync();
    }

    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(DictType entity)
    {
        // 调用 Update
        _db.Set<DictType>().Update(entity);
        // await 异步等待
        await _db.SaveChangesAsync();
    }

    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(DictType entity)
    {
        // 集合操作：移除元素
        _db.Set<DictType>().Remove(entity);
        // await 异步等待
        await _db.SaveChangesAsync();
    }
}
