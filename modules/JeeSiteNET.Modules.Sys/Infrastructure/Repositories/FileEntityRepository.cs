using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class FileEntityRepository : IFileEntityRepository
{
    private readonly JeeSiteDbContext _db;

    public FileEntityRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<FileEntity> Query() => _db.Set<FileEntity>().AsNoTracking();

    public async Task<FileEntity?> GetAsync(string fileId) => await _db.Set<FileEntity>().FindAsync(fileId);

    public async Task<FileEntity?> GetByMd5Async(string md5)
        => await _db.Set<FileEntity>().FirstOrDefaultAsync(f => f.FileMd5 == md5);

    public async Task AddAsync(FileEntity entity) => await _db.Set<FileEntity>().AddAsync(entity);

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
