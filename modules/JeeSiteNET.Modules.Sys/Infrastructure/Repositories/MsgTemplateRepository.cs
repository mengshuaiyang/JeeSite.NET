using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class MsgTemplateRepository : IMsgTemplateRepository
{
    private readonly JeeSiteDbContext _db;
    public MsgTemplateRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<MsgTemplate> Query() => _db.Set<MsgTemplate>().AsNoTracking();

    public async Task<MsgTemplate?> GetAsync(string id) => await _db.Set<MsgTemplate>().FindAsync(id);

    public async Task<MsgTemplate?> GetByKeyAsync(string tplKey)
        => await _db.Set<MsgTemplate>().FirstOrDefaultAsync(t => t.TplKey == tplKey);

    public async Task AddAsync(MsgTemplate entity) => await _db.Set<MsgTemplate>().AddAsync(entity);

    public Task UpdateAsync(MsgTemplate entity) { _db.Set<MsgTemplate>().Update(entity); return Task.CompletedTask; }

    public async Task DeleteAsync(string id)
    {
        var entity = await _db.Set<MsgTemplate>().FindAsync(id);
        if (entity != null) _db.Set<MsgTemplate>().Remove(entity);
    }

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
