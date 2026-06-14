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

// 定义class FileEntityRepository
// 定义类：FileEntityRepository
public class FileEntityRepository : IFileEntityRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;

    // 构造函数 FileEntityRepository
    // 构造函数：FileEntityRepository
    public FileEntityRepository(JeeSiteDbContext db) => _db = db;

    // 方法：Query
    public IQueryable<FileEntity> Query() => _db.Set<FileEntity>().AsNoTracking();

    // 方法：GetAsync
    public async Task<FileEntity?> GetAsync(string fileId) => await _db.Set<FileEntity>().FindAsync(fileId);

    // 方法 GetByMd5Async
    // 方法：GetByMd5Async
    public async Task<FileEntity?> GetByMd5Async(string md5)
        // 数据库操作：异步取首条或默认值
        => await _db.Set<FileEntity>().FirstOrDefaultAsync(f => f.FileMd5 == md5);

    // 方法：AddAsync
    public async Task AddAsync(FileEntity entity) => await _db.Set<FileEntity>().AddAsync(entity);

    // 方法：SaveChangesAsync
    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
