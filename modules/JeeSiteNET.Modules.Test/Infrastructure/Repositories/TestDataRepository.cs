using JeeSiteNET.Modules.Test.Domain.Entities;
using JeeSiteNET.Modules.Test.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Test.Infrastructure.Repositories;

public class TestDataRepository : ITestDataRepository
{
    private readonly JeeSiteDbContext _db;
    public TestDataRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<TestData> Query() => _db.Set<TestData>().AsNoTracking();
    public async Task<TestData?> GetAsync(object id) => await _db.Set<TestData>().FindAsync(id);
    public async Task<List<TestData>> FindListAsync() => await _db.Set<TestData>().AsNoTracking().ToListAsync();
    public async Task AddAsync(TestData entity) { _db.Set<TestData>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(TestData entity) { _db.Set<TestData>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(TestData entity) { _db.Set<TestData>().Remove(entity); await _db.SaveChangesAsync(); }
}
