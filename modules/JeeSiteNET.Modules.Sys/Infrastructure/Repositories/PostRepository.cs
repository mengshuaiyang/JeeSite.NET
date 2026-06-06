using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly JeeSiteDbContext _db;
    public PostRepository(JeeSiteDbContext db) => _db = db;
    public IQueryable<Post> Query() => _db.Set<Post>().AsNoTracking();
    public async Task<Post?> GetAsync(object id) => await _db.Set<Post>().FindAsync(id);
    public async Task<List<Post>> FindListAsync() => await _db.Set<Post>().AsNoTracking().ToListAsync();
    public async Task AddAsync(Post entity) { await _db.Set<Post>().AddAsync(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(Post entity) { _db.Set<Post>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(Post entity) { _db.Set<Post>().Remove(entity); await _db.SaveChangesAsync(); }
}
