using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class MsgInnerRepository : IMsgInnerRepository
{
    private readonly JeeSiteDbContext _db;
    public MsgInnerRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<MsgInner> Query() => _db.Set<MsgInner>().AsNoTracking();

    public async Task<MsgInner?> GetAsync(string id) => await _db.Set<MsgInner>().FindAsync(id);

    public async Task AddAsync(MsgInner entity) => await _db.Set<MsgInner>().AddAsync(entity);

    public Task UpdateAsync(MsgInner entity) { _db.Set<MsgInner>().Update(entity); return Task.CompletedTask; }

    public async Task DeleteAsync(string id)
    {
        var entity = await _db.Set<MsgInner>().FindAsync(id);
        if (entity != null) _db.Set<MsgInner>().Remove(entity);
    }

    public async Task AddRecordAsync(MsgInnerRecord record) => await _db.Set<MsgInnerRecord>().AddAsync(record);

    public async Task AddRecordsAsync(IEnumerable<MsgInnerRecord> records) => await _db.Set<MsgInnerRecord>().AddRangeAsync(records);

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
