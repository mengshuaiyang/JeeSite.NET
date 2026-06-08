using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class FileUploadRepository : IFileUploadRepository
{
    private readonly JeeSiteDbContext _db;

    public FileUploadRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<FileUpload> Query() => _db.Set<FileUpload>().AsNoTracking();

    public async Task<FileUpload?> GetAsync(string id) => await _db.Set<FileUpload>().FindAsync(id);

    public async Task<List<FileUpload>> GetByBizAsync(string bizType, string bizKey)
        => await _db.Set<FileUpload>()
            .Where(f => f.BizType == bizType && f.BizKey == bizKey)
            .OrderBy(f => f.FileSort)
            .ToListAsync();

    public async Task AddAsync(FileUpload entity) => await _db.Set<FileUpload>().AddAsync(entity);

    public Task UpdateAsync(FileUpload entity)
    {
        _db.Set<FileUpload>().Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await _db.Set<FileUpload>().FindAsync(id);
        if (entity != null) _db.Set<FileUpload>().Remove(entity);
    }

    public async Task DeleteByBizAsync(string bizType, string bizKey)
    {
        var list = await _db.Set<FileUpload>()
            .Where(f => f.BizType == bizType && f.BizKey == bizKey)
            .ToListAsync();
        _db.Set<FileUpload>().RemoveRange(list);
    }

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
