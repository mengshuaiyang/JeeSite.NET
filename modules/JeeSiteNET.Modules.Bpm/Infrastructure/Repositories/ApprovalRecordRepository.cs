using JeeSiteNET.Core;
using JeeSiteNET.Modules.Bpm.Domain.Entities;
using JeeSiteNET.Modules.Bpm.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Bpm.Infrastructure.Repositories;

public class ApprovalRecordRepository : IApprovalRecordRepository
{
    private readonly JeeSiteDbContext _db;
    public ApprovalRecordRepository(JeeSiteDbContext db) => _db = db;
    public IQueryable<ApprovalRecord> Query() => _db.Set<ApprovalRecord>().AsNoTracking();
    public async Task<ApprovalRecord?> GetAsync(object id) => await _db.Set<ApprovalRecord>().FindAsync(id);
    public async Task<List<ApprovalRecord>> FindListAsync() => await _db.Set<ApprovalRecord>().AsNoTracking().ToListAsync();
    public async Task AddAsync(ApprovalRecord entity) { _db.Set<ApprovalRecord>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(ApprovalRecord entity) { _db.Set<ApprovalRecord>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(ApprovalRecord entity) { _db.Set<ApprovalRecord>().Remove(entity); await _db.SaveChangesAsync(); }
    public async Task<List<ApprovalRecord>> FindByBusinessKeyAsync(string businessKey)
        => await _db.Set<ApprovalRecord>().Where(r => r.BusinessKey == businessKey).OrderByDescending(r => r.CreateDate).AsNoTracking().ToListAsync();
    public async Task<List<ApprovalRecord>> FindByAssigneeAsync(string assignee)
        => await _db.Set<ApprovalRecord>().Where(r => r.Assignee == assignee).OrderByDescending(r => r.CreateDate).AsNoTracking().ToListAsync();
}
