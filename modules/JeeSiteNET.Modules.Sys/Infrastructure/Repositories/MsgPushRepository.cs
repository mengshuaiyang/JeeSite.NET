using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class MsgPushRepository : IMsgPushRepository
{
    private readonly DbContext _db;
    public MsgPushRepository(DbContext db) => _db = db;

    public IQueryable<MsgPush> Query() => _db.Set<MsgPush>().AsNoTracking();

    public async Task<MsgPush?> GetAsync(string id) => await _db.Set<MsgPush>().FindAsync(id);

    public async Task AddAsync(MsgPush entity) => await _db.Set<MsgPush>().AddAsync(entity);

    public Task UpdateAsync(MsgPush entity) { _db.Set<MsgPush>().Update(entity); return Task.CompletedTask; }

    public async Task DeleteAsync(string id)
    {
        var entity = await _db.Set<MsgPush>().FindAsync(id);
        if (entity != null) _db.Set<MsgPush>().Remove(entity);
    }

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
