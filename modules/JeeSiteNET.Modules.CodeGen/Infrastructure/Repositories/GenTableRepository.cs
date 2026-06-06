using JeeSiteNET.Core;
using JeeSiteNET.Modules.CodeGen.Domain.Entities;
using JeeSiteNET.Modules.CodeGen.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.CodeGen.Infrastructure.Repositories;

public class GenTableRepository : IGenTableRepository
{
    private readonly JeeSiteDbContext _db;
    public GenTableRepository(JeeSiteDbContext db) => _db = db;
    public IQueryable<GenTable> Query() => _db.Set<GenTable>().AsNoTracking();
    public async Task<GenTable?> GetAsync(object id) => await _db.Set<GenTable>().FindAsync(id);
    public async Task<List<GenTable>> FindListAsync() => await _db.Set<GenTable>().AsNoTracking().ToListAsync();
    public async Task AddAsync(GenTable entity) { _db.Set<GenTable>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(GenTable entity) { _db.Set<GenTable>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(GenTable entity) { _db.Set<GenTable>().Remove(entity); await _db.SaveChangesAsync(); }

    public async Task<List<GenTable>> FindListWithColumnsAsync()
        => await _db.Set<GenTable>().Include(e => e.Columns.OrderBy(c => c.ColumnSort)).AsNoTracking().ToListAsync();

    public async Task<GenTable?> GetWithColumnsAsync(string tableName)
        => await _db.Set<GenTable>().Include(e => e.Columns.OrderBy(c => c.ColumnSort)).FirstOrDefaultAsync(e => e.TableName == tableName);

    public async Task<List<GenTableColumn>> GetColumnsAsync(string tableName)
        => await _db.Set<GenTableColumn>().Where(c => c.TableName == tableName).OrderBy(c => c.ColumnSort).AsNoTracking().ToListAsync();
}
