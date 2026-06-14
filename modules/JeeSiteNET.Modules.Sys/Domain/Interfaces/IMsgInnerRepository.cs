    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

// 定义接口 IMsgInnerRepository
// 定义接口：IMsgInnerRepository
public interface IMsgInnerRepository
{
    IQueryable<MsgInner> Query();
    Task<MsgInner?> GetAsync(string id);
    Task AddAsync(MsgInner entity);
    Task UpdateAsync(MsgInner entity);
    Task DeleteAsync(string id);
    Task AddRecordAsync(MsgInnerRecord record);
    Task AddRecordsAsync(IEnumerable<MsgInnerRecord> records);
    Task SaveChangesAsync();
}
