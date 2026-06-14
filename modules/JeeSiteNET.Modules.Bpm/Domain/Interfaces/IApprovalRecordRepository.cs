    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Bpm.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Bpm.Domain.Entities
using JeeSiteNET.Modules.Bpm.Domain.Entities;

// 定义 JeeSiteNET.Modules.Bpm.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.Bpm.Domain.Interfaces
namespace JeeSiteNET.Modules.Bpm.Domain.Interfaces;

// 定义接口 IApprovalRecordRepository
// 定义接口：IApprovalRecordRepository
public interface IApprovalRecordRepository : IRepository<ApprovalRecord>
{
    Task<List<ApprovalRecord>> FindByBusinessKeyAsync(string businessKey);
    Task<List<ApprovalRecord>> FindByAssigneeAsync(string assignee);
}
