using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

public interface IMsgPushRepository
{
    IQueryable<MsgPush> Query();
    Task<MsgPush?> GetAsync(string id);
    Task AddAsync(MsgPush entity);
    Task UpdateAsync(MsgPush entity);
    Task DeleteAsync(string id);
    Task SaveChangesAsync();
}
