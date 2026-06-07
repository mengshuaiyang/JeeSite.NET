using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

public interface IFileEntityRepository
{
    IQueryable<FileEntity> Query();
    Task<FileEntity?> GetAsync(string fileId);
    Task<FileEntity?> GetByMd5Async(string md5);
    Task AddAsync(FileEntity entity);
    Task SaveChangesAsync();
}
