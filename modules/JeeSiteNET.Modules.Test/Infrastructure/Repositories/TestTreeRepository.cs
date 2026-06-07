using JeeSiteNET.Modules.Test.Domain.Entities;
using JeeSiteNET.Modules.Test.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Test.Infrastructure.Repositories;

public class TestTreeRepository : ITestTreeRepository
{
    private readonly JeeSiteDbContext _db;
    public TestTreeRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<TestTree> Query() => _db.Set<TestTree>().AsNoTracking();
    public async Task<TestTree?> GetAsync(object id) => await _db.Set<TestTree>().FindAsync(id);
    public async Task<List<TestTree>> FindListAsync() => await _db.Set<TestTree>().AsNoTracking().ToListAsync();
    public async Task AddAsync(TestTree entity) { _db.Set<TestTree>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(TestTree entity) { _db.Set<TestTree>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(TestTree entity) { _db.Set<TestTree>().Remove(entity); await _db.SaveChangesAsync(); }
}
