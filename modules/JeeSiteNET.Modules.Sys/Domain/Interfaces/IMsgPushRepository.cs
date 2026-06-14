    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

// 定义接口 IMsgPushRepository
// 定义接口：IMsgPushRepository
public interface IMsgPushRepository
{
    IQueryable<MsgPush> Query();
    Task<MsgPush?> GetAsync(string id);
    Task AddAsync(MsgPush entity);
    Task UpdateAsync(MsgPush entity);
    Task DeleteAsync(string id);
    Task SaveChangesAsync();
}
