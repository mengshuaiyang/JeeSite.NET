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

// 定义class OrganizationRepository
// 定义类：OrganizationRepository
public class OrganizationRepository : IOrganizationRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;

    // 构造函数 OrganizationRepository
    // 构造函数：OrganizationRepository
    public OrganizationRepository(JeeSiteDbContext db) => _db = db;

    // 方法：Query
    public IQueryable<Organization> Query() => _db.Set<Organization>().AsNoTracking();

    // 方法 GetAsync
    // 方法：GetAsync
    public async Task<Organization?> GetAsync(object id)
        => await _db.Set<Organization>().FindAsync(id);

    // 方法 FindListAsync
    // 方法：FindListAsync
    public async Task<List<Organization>> FindListAsync()
        // 数据库操作：不跟踪查询（只读）
        => await _db.Set<Organization>().AsNoTracking().ToListAsync();

    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(Organization entity)
    {
        // await 异步等待
        await _db.Set<Organization>().AddAsync(entity);
        // await 异步等待
        await _db.SaveChangesAsync();
    }

    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(Organization entity)
    {
        // 调用 Update
        _db.Set<Organization>().Update(entity);
        // await 异步等待
        await _db.SaveChangesAsync();
    }

    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(Organization entity)
    {
        // 集合操作：移除元素
        _db.Set<Organization>().Remove(entity);
        // await 异步等待
        await _db.SaveChangesAsync();
    }
}
