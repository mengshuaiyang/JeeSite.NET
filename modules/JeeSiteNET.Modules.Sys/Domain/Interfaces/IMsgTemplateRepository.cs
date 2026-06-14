    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

// 定义接口 IMsgTemplateRepository
// 定义接口：IMsgTemplateRepository
public interface IMsgTemplateRepository
{
    IQueryable<MsgTemplate> Query();
    Task<MsgTemplate?> GetAsync(string id);
    Task<MsgTemplate?> GetByKeyAsync(string tplKey);
    Task AddAsync(MsgTemplate entity);
    Task UpdateAsync(MsgTemplate entity);
    Task DeleteAsync(string id);
    Task SaveChangesAsync();
}
