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

// 定义class GenTableColumnRepository
// 定义类：GenTableColumnRepository
public class GenTableColumnRepository : IGenTableColumnRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 GenTableColumnRepository
    // 构造函数：GenTableColumnRepository
    public GenTableColumnRepository(JeeSiteDbContext db) => _db = db;
    // 方法：Query
    public IQueryable<GenTableColumn> Query() => _db.Set<GenTableColumn>().AsNoTracking();
    // 方法：GetAsync
    public async Task<GenTableColumn?> GetAsync(object id) => await _db.Set<GenTableColumn>().FindAsync(id);
    // 方法：FindListAsync
    public async Task<List<GenTableColumn>> FindListAsync() => await _db.Set<GenTableColumn>().AsNoTracking().ToListAsync();
    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(GenTableColumn entity) { _db.Set<GenTableColumn>().Add(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(GenTableColumn entity) { _db.Set<GenTableColumn>().Update(entity); await _db.SaveChangesAsync(); }
    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(GenTableColumn entity) { _db.Set<GenTableColumn>().Remove(entity); await _db.SaveChangesAsync(); }

    // 方法 RemoveAsync：仅标记删除，不提交
    public Task RemoveAsync(GenTableColumn entity) { _db.Set<GenTableColumn>().Remove(entity); return Task.CompletedTask; }

    // 方法 SaveChangesAsync：统一提交挂起的变更
    public Task SaveChangesAsync() => _db.SaveChangesAsync();
    // 方法 FindByTableNameAsync
    // 方法：FindByTableNameAsync
    public async Task<List<GenTableColumn>> FindByTableNameAsync(string tableName)
        // 数据库操作：条件过滤
        => await _db.Set<GenTableColumn>().Where(c => c.TableName == tableName).OrderBy(c => c.ColumnSort).AsNoTracking().ToListAsync();
}
