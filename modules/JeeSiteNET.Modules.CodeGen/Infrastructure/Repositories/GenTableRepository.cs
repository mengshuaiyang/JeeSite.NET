    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.CodeGen.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.CodeGen.Domain.Entities
using JeeSiteNET.Modules.CodeGen.Domain.Entities;
    // 引入 JeeSiteNET.Modules.CodeGen.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.CodeGen.Domain.Interfaces
using JeeSiteNET.Modules.CodeGen.Domain.Interfaces;
    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.CodeGen.Infrastructure.Repositories 命名空间
// 定义命名空间：JeeSiteNET.Modules.CodeGen.Infrastructure.Repositories
namespace JeeSiteNET.Modules.CodeGen.Infrastructure.Repositories;

// 定义class GenTableRepository
// 定义类：GenTableRepository
public class GenTableRepository : IGenTableRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 GenTableRepository
    // 构造函数：GenTableRepository
    public GenTableRepository(JeeSiteDbContext db) => _db = db;
    // 方法：Query
    public IQueryable<GenTable> Query() => _db.Set<GenTable>().AsNoTracking();
    // 方法：GetAsync
    public async Task<GenTable?> GetAsync(object id) => await _db.Set<GenTable>().FindAsync(id);
    // 方法：FindListAsync
    public async Task<List<GenTable>> FindListAsync() => await _db.Set<GenTable>().AsNoTracking().ToListAsync();
    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(GenTable entity) { _db.Set<GenTable>().Add(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(GenTable entity) { _db.Set<GenTable>().Update(entity); await _db.SaveChangesAsync(); }
    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(GenTable entity) { _db.Set<GenTable>().Remove(entity); await _db.SaveChangesAsync(); }

    // 方法 FindListWithColumnsAsync
    // 方法：FindListWithColumnsAsync
    public async Task<List<GenTable>> FindListWithColumnsAsync()
        // 数据库操作：加载关联实体
        => await _db.Set<GenTable>().Include(e => e.Columns.OrderBy(c => c.ColumnSort)).AsNoTracking().ToListAsync();

    // 方法 GetWithColumnsAsync
    // 方法：GetWithColumnsAsync
    public async Task<GenTable?> GetWithColumnsAsync(string tableName)
        // 数据库操作：加载关联实体
        => await _db.Set<GenTable>().Include(e => e.Columns.OrderBy(c => c.ColumnSort)).AsNoTracking().FirstOrDefaultAsync(e => e.TableName == tableName);

    // 方法 GetColumnsAsync
    // 方法：GetColumnsAsync
    public async Task<List<GenTableColumn>> GetColumnsAsync(string tableName)
        // 数据库操作：条件过滤
        => await _db.Set<GenTableColumn>().Where(c => c.TableName == tableName).OrderBy(c => c.ColumnSort).AsNoTracking().ToListAsync();
}
