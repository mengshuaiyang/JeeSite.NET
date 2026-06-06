using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class DictTypeRepository : IDictTypeRepository
{
    private readonly JeeSiteDbContext _db;

    public DictTypeRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<DictType> Query() => _db.Set<DictType>().AsNoTracking();

    public async Task<DictType?> GetAsync(object id)
        => await _db.Set<DictType>().FindAsync(id);

    public async Task<List<DictType>> FindListAsync()
        => await _db.Set<DictType>().AsNoTracking().ToListAsync();

    public async Task AddAsync(DictType entity)
    {
        await _db.Set<DictType>().AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(DictType entity)
    {
        _db.Set<DictType>().Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(DictType entity)
    {
        _db.Set<DictType>().Remove(entity);
        await _db.SaveChangesAsync();
    }
}
