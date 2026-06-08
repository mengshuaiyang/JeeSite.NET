using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Modules.Bpm.Domain.Entities;
using JeeSiteNET.Modules.Bpm.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Bpm.Infrastructure.Repositories;

public class LeaveRepository : ILeaveRepository
{
    private readonly JeeSiteDbContext _db;
    public LeaveRepository(JeeSiteDbContext db) => _db = db;
    public IQueryable<LeaveRequest> Query() => _db.Set<LeaveRequest>().AsNoTracking();
    public async Task<LeaveRequest?> GetAsync(string id) => await _db.Set<LeaveRequest>().FindAsync(id);
    public async Task<List<LeaveRequest>> FindListAsync() => await _db.Set<LeaveRequest>().AsNoTracking().ToListAsync();
    public async Task<List<LeaveRequest>> FindByApplicantAsync(string applicant)
        => await _db.Set<LeaveRequest>().Where(l => l.Applicant == applicant).OrderByDescending(l => l.CreateDate).ToListAsync();
    public async Task<List<LeaveRequest>> FindByApproverAsync(string userCode)
        => await _db.Set<LeaveRequest>().Where(l => l.ManagerApprover == userCode || l.HrApprover == userCode).OrderByDescending(l => l.CreateDate).ToListAsync();
    public async Task AddAsync(LeaveRequest entity) { _db.Set<LeaveRequest>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(LeaveRequest entity) { _db.Set<LeaveRequest>().Update(entity); await _db.SaveChangesAsync(); }
}
