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

// 定义class DictDataRepository
// 定义类：DictDataRepository
public class DictDataRepository : IDictDataRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;

    // 构造函数 DictDataRepository
    // 构造函数：DictDataRepository
    public DictDataRepository(JeeSiteDbContext db) => _db = db;

    // 方法：Query
    public IQueryable<DictData> Query() => _db.Set<DictData>().AsNoTracking();

    // 方法 GetAsync
    // 方法：GetAsync
    public async Task<DictData?> GetAsync(object id)
        => await _db.Set<DictData>().FindAsync(id);

    // 方法 FindListAsync
    // 方法：FindListAsync
    public async Task<List<DictData>> FindListAsync()
        // 数据库操作：不跟踪查询（只读）
        => await _db.Set<DictData>().AsNoTracking().ToListAsync();

    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(DictData entity)
    {
        // await 异步等待
        await _db.Set<DictData>().AddAsync(entity);
        // await 异步等待
        await _db.SaveChangesAsync();
    }

    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(DictData entity)
    {
        // 调用 Update
        _db.Set<DictData>().Update(entity);
        // await 异步等待
        await _db.SaveChangesAsync();
    }

    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(DictData entity)
    {
        // 集合操作：移除元素
        _db.Set<DictData>().Remove(entity);
        // await 异步等待
        await _db.SaveChangesAsync();
    }

    // 方法 GetByTypeAsync
    // 方法：GetByTypeAsync
    public async Task<List<DictData>> GetByTypeAsync(string dictType)
        // 数据库操作：不跟踪查询（只读）
        => await _db.Set<DictData>().AsNoTracking()
            // 数据库操作：条件过滤
            .Where(d => d.DictType == dictType && d.Status == "0")
            // 数据库操作：升序排序
            .OrderBy(d => d.Sort)
            // 数据库操作：异步查询为列表
            .ToListAsync();
}
