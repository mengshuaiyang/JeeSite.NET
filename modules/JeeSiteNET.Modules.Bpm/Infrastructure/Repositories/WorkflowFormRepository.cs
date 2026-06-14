    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Bpm.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Domain.Entities
using JeeSiteNET.Modules.Bpm.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Bpm.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Domain.Interfaces
using JeeSiteNET.Modules.Bpm.Domain.Interfaces;
    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.Bpm.Infrastructure.Repositories 命名空间
// 定义命名空间：JeeSiteNET.Modules.Bpm.Infrastructure.Repositories
namespace JeeSiteNET.Modules.Bpm.Infrastructure.Repositories;

// 定义class WorkflowFormRepository
// 定义类：WorkflowFormRepository
public class WorkflowFormRepository : IWorkflowFormRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 WorkflowFormRepository
    // 构造函数：WorkflowFormRepository
    public WorkflowFormRepository(JeeSiteDbContext db) => _db = db;
    // 方法：Query
    public IQueryable<WorkflowForm> Query() => _db.Set<WorkflowForm>().AsNoTracking();
    // 方法：GetAsync
    public async Task<WorkflowForm?> GetAsync(object id) => await _db.Set<WorkflowForm>().FindAsync(id);
    // 方法：FindListAsync
    public async Task<List<WorkflowForm>> FindListAsync() => await _db.Set<WorkflowForm>().AsNoTracking().ToListAsync();
    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(WorkflowForm entity) { _db.Set<WorkflowForm>().Add(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(WorkflowForm entity) { _db.Set<WorkflowForm>().Update(entity); await _db.SaveChangesAsync(); }
    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(WorkflowForm entity) { _db.Set<WorkflowForm>().Remove(entity); await _db.SaveChangesAsync(); }
    // 方法 FindByBusinessKeyAsync
    // 方法：FindByBusinessKeyAsync
    public async Task<WorkflowForm?> FindByBusinessKeyAsync(string businessKey)
        // 数据库操作：异步取首条或默认值
        => await _db.Set<WorkflowForm>().FirstOrDefaultAsync(f => f.BusinessKey == businessKey);
}
