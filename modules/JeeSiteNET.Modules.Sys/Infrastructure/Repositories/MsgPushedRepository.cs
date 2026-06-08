using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class MsgPushedRepository : IMsgPushedRepository
{
    private readonly JeeSiteDbContext _db;
    public MsgPushedRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<MsgPushed> Query() => _db.Set<MsgPushed>().AsNoTracking();

    public async Task<MsgPushed?> GetAsync(string id) => await _db.Set<MsgPushed>().FindAsync(id);

    public async Task AddAsync(MsgPushed entity) => await _db.Set<MsgPushed>().AddAsync(entity);

    public Task UpdateAsync(MsgPushed entity) { _db.Set<MsgPushed>().Update(entity); return Task.CompletedTask; }

    public async Task DeleteAsync(string id)
    {
        var entity = await _db.Set<MsgPushed>().FindAsync(id);
        if (entity != null) _db.Set<MsgPushed>().Remove(entity);
    }

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
