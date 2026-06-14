    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

// 定义接口 IMsgPushedRepository
// 定义接口：IMsgPushedRepository
public interface IMsgPushedRepository
{
    IQueryable<MsgPushed> Query();
    Task<MsgPushed?> GetAsync(string id);
    Task AddAsync(MsgPushed entity);
    Task UpdateAsync(MsgPushed entity);
    Task DeleteAsync(string id);
    Task SaveChangesAsync();
}
