using JeeSiteNET.Core;
using JeeSiteNET.Modules.CodeGen.Domain.Entities;
using JeeSiteNET.Modules.CodeGen.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.CodeGen.Infrastructure.Repositories;

public class GenTableColumnRepository : IGenTableColumnRepository
{
    private readonly JeeSiteDbContext _db;
    public GenTableColumnRepository(JeeSiteDbContext db) => _db = db;
    public IQueryable<GenTableColumn> Query() => _db.Set<GenTableColumn>().AsNoTracking();
    public async Task<GenTableColumn?> GetAsync(object id) => await _db.Set<GenTableColumn>().FindAsync(id);
    public async Task<List<GenTableColumn>> FindListAsync() => await _db.Set<GenTableColumn>().AsNoTracking().ToListAsync();
    public async Task AddAsync(GenTableColumn entity) { _db.Set<GenTableColumn>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(GenTableColumn entity) { _db.Set<GenTableColumn>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(GenTableColumn entity) { _db.Set<GenTableColumn>().Remove(entity); await _db.SaveChangesAsync(); }
    public async Task<List<GenTableColumn>> FindByTableNameAsync(string tableName)
        => await _db.Set<GenTableColumn>().Where(c => c.TableName == tableName).OrderBy(c => c.ColumnSort).AsNoTracking().ToListAsync();
}
