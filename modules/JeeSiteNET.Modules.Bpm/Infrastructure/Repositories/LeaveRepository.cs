    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
    // 引入 JeeSiteNET.Modules.Bpm.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Domain.Entities
using JeeSiteNET.Modules.Bpm.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Bpm.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Domain.Interfaces
using JeeSiteNET.Modules.Bpm.Domain.Interfaces;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.Bpm.Infrastructure.Repositories 命名空间
// 定义命名空间：JeeSiteNET.Modules.Bpm.Infrastructure.Repositories
namespace JeeSiteNET.Modules.Bpm.Infrastructure.Repositories;

// 定义class LeaveRepository
// 定义类：LeaveRepository
public class LeaveRepository : ILeaveRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 LeaveRepository
    // 构造函数：LeaveRepository
    public LeaveRepository(JeeSiteDbContext db) => _db = db;
    // 方法：Query
    public IQueryable<LeaveRequest> Query() => _db.Set<LeaveRequest>().AsNoTracking();
    // 方法：GetAsync
    public async Task<LeaveRequest?> GetAsync(string id) => await _db.Set<LeaveRequest>().FindAsync(id);
    // 方法：FindListAsync
    public async Task<List<LeaveRequest>> FindListAsync() => await _db.Set<LeaveRequest>().AsNoTracking().ToListAsync();
    // 方法 FindByApplicantAsync
    // 方法：FindByApplicantAsync
    public async Task<List<LeaveRequest>> FindByApplicantAsync(string applicant)
        // 数据库操作：条件过滤
        => await _db.Set<LeaveRequest>().Where(l => l.Applicant == applicant).OrderByDescending(l => l.CreateDate).ToListAsync();
    // 方法 FindByApproverAsync
    // 方法：FindByApproverAsync
    public async Task<List<LeaveRequest>> FindByApproverAsync(string userCode)
        // 数据库操作：条件过滤
        => await _db.Set<LeaveRequest>().Where(l => l.ManagerApprover == userCode || l.HrApprover == userCode).OrderByDescending(l => l.CreateDate).ToListAsync();
    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(LeaveRequest entity) { _db.Set<LeaveRequest>().Add(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(LeaveRequest entity) { _db.Set<LeaveRequest>().Update(entity); await _db.SaveChangesAsync(); }
}
