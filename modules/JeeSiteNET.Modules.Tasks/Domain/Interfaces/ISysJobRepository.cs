    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Tasks.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Tasks.Domain.Entities
using JeeSiteNET.Modules.Tasks.Domain.Entities;

// 定义 JeeSiteNET.Modules.Tasks.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.Tasks.Domain.Interfaces
namespace JeeSiteNET.Modules.Tasks.Domain.Interfaces;

// 定义接口 ISysJobRepository
// 定义接口：ISysJobRepository
public interface ISysJobRepository : IRepository<SysJob>
{
    Task<List<SysJob>> FindRunningJobsAsync();
}
