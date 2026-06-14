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

// 定义class ApprovalRecordRepository
// 定义类：ApprovalRecordRepository
public class ApprovalRecordRepository : IApprovalRecordRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数：ApprovalRecordRepository
    public ApprovalRecordRepository(JeeSiteDbContext db) => _db = db;
    // 方法：Query
    public IQueryable<ApprovalRecord> Query() => _db.Set<ApprovalRecord>().AsNoTracking();
    // 方法：GetAsync
    public async Task<ApprovalRecord?> GetAsync(object id) => await _db.Set<ApprovalRecord>().FindAsync(id);
    // 方法：FindListAsync
    public async Task<List<ApprovalRecord>> FindListAsync() => await _db.Set<ApprovalRecord>().AsNoTracking().ToListAsync();
    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(ApprovalRecord entity) { _db.Set<ApprovalRecord>().Add(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(ApprovalRecord entity) { _db.Set<ApprovalRecord>().Update(entity); await _db.SaveChangesAsync(); }
    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(ApprovalRecord entity) { _db.Set<ApprovalRecord>().Remove(entity); await _db.SaveChangesAsync(); }
    // 方法 FindByBusinessKeyAsync
    // 方法：FindByBusinessKeyAsync
    public async Task<List<ApprovalRecord>> FindByBusinessKeyAsync(string businessKey)
        // 数据库操作：条件过滤
        => await _db.Set<ApprovalRecord>().Where(r => r.BusinessKey == businessKey).OrderByDescending(r => r.CreateDate).AsNoTracking().ToListAsync();
    // 方法 FindByAssigneeAsync
    // 方法：FindByAssigneeAsync
    public async Task<List<ApprovalRecord>> FindByAssigneeAsync(string assignee)
        // 数据库操作：条件过滤
        => await _db.Set<ApprovalRecord>().Where(r => r.Assignee == assignee).OrderByDescending(r => r.CreateDate).AsNoTracking().ToListAsync();
}
