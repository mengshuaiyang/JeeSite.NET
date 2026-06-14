    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Tasks.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Tasks.Domain.Entities
using JeeSiteNET.Modules.Tasks.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Tasks.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Tasks.Domain.Interfaces
using JeeSiteNET.Modules.Tasks.Domain.Interfaces;
    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.Tasks.Infrastructure.Repositories 命名空间
// 定义命名空间：JeeSiteNET.Modules.Tasks.Infrastructure.Repositories
namespace JeeSiteNET.Modules.Tasks.Infrastructure.Repositories;

// 定义class SysJobRepository
// 定义类：SysJobRepository
public class SysJobRepository : ISysJobRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 SysJobRepository
    // 构造函数：SysJobRepository
    public SysJobRepository(JeeSiteDbContext db) => _db = db;
    // 方法：Query
    public IQueryable<SysJob> Query() => _db.Set<SysJob>().AsNoTracking();
    // 方法：GetAsync
    public async Task<SysJob?> GetAsync(object id) => await _db.Set<SysJob>().FindAsync(id);
    // 方法：FindListAsync
    public async Task<List<SysJob>> FindListAsync() => await _db.Set<SysJob>().AsNoTracking().ToListAsync();
    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(SysJob entity) { _db.Set<SysJob>().Add(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(SysJob entity) { _db.Set<SysJob>().Update(entity); await _db.SaveChangesAsync(); }
    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(SysJob entity) { _db.Set<SysJob>().Remove(entity); await _db.SaveChangesAsync(); }
    // 方法：FindRunningJobsAsync
    public async Task<List<SysJob>> FindRunningJobsAsync() => await _db.Set<SysJob>().Where(j => j.RunStatus == "1" && j.Status == "0").AsNoTracking().ToListAsync();
}
