using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

public interface IMsgPushedRepository
{
    IQueryable<MsgPushed> Query();
    Task<MsgPushed?> GetAsync(string id);
    Task AddAsync(MsgPushed entity);
    Task UpdateAsync(MsgPushed entity);
    Task DeleteAsync(string id);
    Task SaveChangesAsync();
}
