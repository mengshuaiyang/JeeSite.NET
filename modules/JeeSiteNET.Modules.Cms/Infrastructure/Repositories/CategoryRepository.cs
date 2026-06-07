using JeeSiteNET.Core;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Cms.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly JeeSiteDbContext _db;
    public CategoryRepository(JeeSiteDbContext db) => _db = db;
    public IQueryable<Category> Query() => _db.Set<Category>().AsNoTracking();
    public async Task<Category?> GetAsync(object id) => await _db.Set<Category>().FindAsync(id);
    public async Task<List<Category>> FindListAsync() => await _db.Set<Category>().AsNoTracking().ToListAsync();
    public async Task AddAsync(Category entity) { _db.Set<Category>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(Category entity) { _db.Set<Category>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(Category entity) { _db.Set<Category>().Remove(entity); await _db.SaveChangesAsync(); }
    public async Task<List<Category>> FindTreeAsync() => await _db.Set<Category>().AsNoTracking().OrderBy(e => e.TreeSort).ToListAsync();
    public async Task<List<Category>> FindByCodesAsync(List<string> categoryCodes) => await _db.Set<Category>().AsNoTracking().Where(e => categoryCodes.Contains(e.CategoryCode)).ToListAsync();
}
