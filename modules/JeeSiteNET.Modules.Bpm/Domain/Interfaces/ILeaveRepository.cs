using JeeSiteNET.Modules.Bpm.Domain.Entities;

namespace JeeSiteNET.Modules.Bpm.Domain.Interfaces;

public interface ILeaveRepository
{
    IQueryable<LeaveRequest> Query();
    Task<LeaveRequest?> GetAsync(string id);
    Task<List<LeaveRequest>> FindListAsync();
    Task<List<LeaveRequest>> FindByApplicantAsync(string applicant);
    Task<List<LeaveRequest>> FindByApproverAsync(string userCode);
    Task AddAsync(LeaveRequest entity);
    Task UpdateAsync(LeaveRequest entity);
}
