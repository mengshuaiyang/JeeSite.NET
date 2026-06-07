using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

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
