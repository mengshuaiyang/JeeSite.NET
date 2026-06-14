    // 引入 JeeSiteNET.Modules.Bpm.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Domain.Entities
using JeeSiteNET.Modules.Bpm.Domain.Entities;

// 定义 JeeSiteNET.Modules.Bpm.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.Bpm.Domain.Interfaces
namespace JeeSiteNET.Modules.Bpm.Domain.Interfaces;

// 定义接口 ILeaveRepository
// 定义接口：ILeaveRepository
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
