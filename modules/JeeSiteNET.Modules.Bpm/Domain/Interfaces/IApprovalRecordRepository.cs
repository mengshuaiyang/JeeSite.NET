using JeeSiteNET.Core;
using JeeSiteNET.Modules.Bpm.Domain.Entities;

namespace JeeSiteNET.Modules.Bpm.Domain.Interfaces;

public interface IApprovalRecordRepository : IRepository<ApprovalRecord>
{
    Task<List<ApprovalRecord>> FindByBusinessKeyAsync(string businessKey);
    Task<List<ApprovalRecord>> FindByAssigneeAsync(string assignee);
}
