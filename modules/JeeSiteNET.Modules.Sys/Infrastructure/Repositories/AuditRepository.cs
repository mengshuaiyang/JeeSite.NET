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

// 定义class AuditRepository
// 定义类：AuditRepository
public class AuditRepository : IAuditRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 AuditRepository
    // 构造函数：AuditRepository
    public AuditRepository(JeeSiteDbContext db) => _db = db;
    // 方法：Query
    public IQueryable<Audit> Query() => _db.Set<Audit>().AsNoTracking();
    // 方法：GetAsync
    public async Task<Audit?> GetAsync(object id) => await _db.Set<Audit>().FindAsync(id);
    // 方法：FindListAsync
    public async Task<List<Audit>> FindListAsync() => await _db.Set<Audit>().AsNoTracking().ToListAsync();
    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(Audit entity) { await _db.Set<Audit>().AddAsync(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(Audit entity) { _db.Set<Audit>().Update(entity); await _db.SaveChangesAsync(); }
    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(Audit entity) { _db.Set<Audit>().Remove(entity); await _db.SaveChangesAsync(); }
}
