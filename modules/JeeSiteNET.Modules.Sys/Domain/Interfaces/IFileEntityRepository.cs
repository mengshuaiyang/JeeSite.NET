    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

// 定义接口 IFileEntityRepository
// 定义接口：IFileEntityRepository
public interface IFileEntityRepository
{
    IQueryable<FileEntity> Query();
    Task<FileEntity?> GetAsync(string fileId);
    Task<FileEntity?> GetByMd5Async(string md5);
    Task AddAsync(FileEntity entity);
    Task SaveChangesAsync();
}
