    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
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

// 定义class TenantRepository
// 定义类：TenantRepository
public class TenantRepository : ITenantRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;

    // 构造函数 TenantRepository
    // 构造函数：TenantRepository
    public TenantRepository(JeeSiteDbContext db) => _db = db;

    // 方法：Query
    public IQueryable<Tenant> Query() => _db.Set<Tenant>();

    // 方法 GetAsync
    // 方法：GetAsync
    public async Task<Tenant?> GetAsync(object id)
        // 数据库操作：异步取首条或默认值
        => await _db.Set<Tenant>().FirstOrDefaultAsync(e => e.TenantCode == id.ToString());

    // 方法 FindListAsync
    // 方法：FindListAsync
    public async Task<List<Tenant>> FindListAsync()
        // 数据库操作：异步查询为列表
        => await _db.Set<Tenant>().ToListAsync();

    // 方法 FindPageAsync
    // 方法：FindPageAsync
    public async Task<PageResult<Tenant>> FindPageAsync(PageRequest<Tenant> request)
    {
        // 声明并初始化变量：query
        var query = _db.Set<Tenant>().AsQueryable();
        // 数据库操作：异步统计数量
        var total = await query.CountAsync();
        // 数据库操作：升序排序
        var list = await query.OrderBy(e => e.TenantCode)
            // 调用 Skip
            .Skip((request.PageNo - 1) * request.PageSize)
            // 数据库操作：异步查询为列表
            .Take(request.PageSize).ToListAsync();
        // return 返回结果
        return new PageResult<Tenant> { List = list, Total = total, PageNo = request.PageNo, PageSize = request.PageSize };
    }

    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(Tenant entity) { await _db.Set<Tenant>().AddAsync(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(Tenant entity) { _db.Set<Tenant>().Update(entity); await _db.SaveChangesAsync(); }
    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(Tenant entity) { _db.Set<Tenant>().Remove(entity); await _db.SaveChangesAsync(); }
}
