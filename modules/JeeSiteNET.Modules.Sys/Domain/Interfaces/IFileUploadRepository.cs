    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

// 定义接口 IFileUploadRepository
// 定义接口：IFileUploadRepository
public interface IFileUploadRepository
{
    IQueryable<FileUpload> Query();
    Task<FileUpload?> GetAsync(string id);
    Task<FileUpload?> GetByFileIdAsync(string fileId);
    Task<List<FileUpload>> GetByBizAsync(string bizType, string bizKey);
    Task AddAsync(FileUpload entity);
    Task UpdateAsync(FileUpload entity);
    Task DeleteAsync(string id);
    Task DeleteByBizAsync(string bizType, string bizKey);
    Task SaveChangesAsync();
}
