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

// 定义class JobLogRepository
// 定义类：JobLogRepository
public class JobLogRepository : IJobLogRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 JobLogRepository
    // 构造函数：JobLogRepository
    public JobLogRepository(JeeSiteDbContext db) => _db = db;
    // 方法：Query
    public IQueryable<JobLog> Query() => _db.Set<JobLog>().AsNoTracking();
    // 方法：GetAsync
    public async Task<JobLog?> GetAsync(object id) => await _db.Set<JobLog>().FindAsync(id);
    // 方法：FindListAsync
    public async Task<List<JobLog>> FindListAsync() => await _db.Set<JobLog>().AsNoTracking().ToListAsync();
    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(JobLog entity) { _db.Set<JobLog>().Add(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(JobLog entity) { _db.Set<JobLog>().Update(entity); await _db.SaveChangesAsync(); }
    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(JobLog entity) { _db.Set<JobLog>().Remove(entity); await _db.SaveChangesAsync(); }
    // 方法 FindByJobIdAsync
    // 方法：FindByJobIdAsync
    public async Task<List<JobLog>> FindByJobIdAsync(string jobId, int limit = 50)
        // 数据库操作：条件过滤
        => await _db.Set<JobLog>().Where(l => l.JobId == jobId).OrderByDescending(l => l.RunDate).Take(limit).AsNoTracking().ToListAsync();
}
