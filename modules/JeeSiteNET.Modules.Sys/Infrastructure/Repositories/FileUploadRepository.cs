    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.Sys.Infrastructure.Repositories 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Infrastructure.Repositories
namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

// 定义class FileUploadRepository
// 定义类：FileUploadRepository
public class FileUploadRepository : IFileUploadRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;

    // 构造函数 FileUploadRepository
    // 构造函数：FileUploadRepository
    public FileUploadRepository(JeeSiteDbContext db) => _db = db;

    // 方法：Query
    public IQueryable<FileUpload> Query() => _db.Set<FileUpload>().AsNoTracking();

    // 方法：GetAsync
    public async Task<FileUpload?> GetAsync(string id) => await _db.Set<FileUpload>().FindAsync(id);

    // 方法 GetByFileIdAsync
    // 方法：GetByFileIdAsync
    public async Task<FileUpload?> GetByFileIdAsync(string fileId)
        // 数据库操作：异步取首条或默认值
        => await _db.Set<FileUpload>().FirstOrDefaultAsync(f => f.FileId == fileId);

    // 方法 GetByBizAsync
    // 方法：GetByBizAsync
    public async Task<List<FileUpload>> GetByBizAsync(string bizType, string bizKey)
        => await _db.Set<FileUpload>()
            // 数据库操作：条件过滤
            .Where(f => f.BizType == bizType && f.BizKey == bizKey)
            // 数据库操作：升序排序
            .OrderBy(f => f.FileSort)
            // 数据库操作：异步查询为列表
            .ToListAsync();

    // 方法：AddAsync
    public async Task AddAsync(FileUpload entity) => await _db.Set<FileUpload>().AddAsync(entity);

    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public Task UpdateAsync(FileUpload entity)
    {
        // 调用 Update
        _db.Set<FileUpload>().Update(entity);
        // return 返回结果
        return Task.CompletedTask;
    }

    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(string id)
    {
        var entity = await _db.Set<FileUpload>().FindAsync(id);
        // if 条件判断
        if (entity != null) _db.Set<FileUpload>().Remove(entity);
    }

    // 方法 DeleteByBizAsync
    // 方法：DeleteByBizAsync
    public async Task DeleteByBizAsync(string bizType, string bizKey)
    {
        var list = await _db.Set<FileUpload>()
            // 数据库操作：条件过滤
            .Where(f => f.BizType == bizType && f.BizKey == bizKey)
            // 数据库操作：异步查询为列表
            .ToListAsync();
        // 集合操作：批量移除
        _db.Set<FileUpload>().RemoveRange(list);
    }

    // 方法：SaveChangesAsync
    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
