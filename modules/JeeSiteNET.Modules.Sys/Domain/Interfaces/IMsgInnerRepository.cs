using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

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
