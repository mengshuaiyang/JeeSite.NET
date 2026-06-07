using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

public interface IFileUploadRepository
{
    IQueryable<FileUpload> Query();
    Task<FileUpload?> GetAsync(string id);
    Task<List<FileUpload>> GetByBizAsync(string bizType, string bizKey);
    Task AddAsync(FileUpload entity);
    Task UpdateAsync(FileUpload entity);
    Task DeleteAsync(string id);
    Task DeleteByBizAsync(string bizType, string bizKey);
    Task SaveChangesAsync();
}
