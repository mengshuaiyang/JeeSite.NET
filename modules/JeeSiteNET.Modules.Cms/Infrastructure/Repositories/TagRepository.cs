using JeeSiteNET.Modules.Cms.Domain.Entities;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Cms.Infrastructure.Repositories;

public class TagRepository : ITagRepository
{
    private readonly JeeSiteDbContext _db;
    public TagRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<Tag> Query() => _db.Set<Tag>().AsNoTracking();
    public async Task<Tag?> GetAsync(object id) => await _db.Set<Tag>().FindAsync(id);
    public async Task<List<Tag>> FindListAsync() => await _db.Set<Tag>().AsNoTracking().ToListAsync();
    public async Task AddAsync(Tag entity) { _db.Set<Tag>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(Tag entity) { _db.Set<Tag>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(Tag entity) { _db.Set<Tag>().Remove(entity); await _db.SaveChangesAsync(); }
}
