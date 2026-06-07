using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly DbContext _db;

    public EmployeeRepository(DbContext db)
    {
        _db = db;
    }

    public IQueryable<Employee> Query() => _db.Set<Employee>().AsNoTracking();

    public async Task<Employee?> GetAsync(string empCode)
        => await _db.Set<Employee>().FindAsync(empCode);

    public async Task<List<EmployeePost>> GetPostsAsync(string empCode)
        => await _db.Set<EmployeePost>().Where(e => e.EmpCode == empCode).AsNoTracking().ToListAsync();

    public async Task<List<EmployeeOffice>> GetOfficesAsync(string empCode)
        => await _db.Set<EmployeeOffice>().Where(e => e.EmpCode == empCode).AsNoTracking().ToListAsync();

    public async Task AddAsync(Employee entity)
        => await _db.Set<Employee>().AddAsync(entity);

    public Task UpdateAsync(Employee entity)
    {
        _db.Set<Employee>().Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(string empCode)
    {
        var entity = await _db.Set<Employee>().FindAsync(empCode);
        if (entity != null) _db.Set<Employee>().Remove(entity);
        await DeletePostsAsync(empCode);
        await DeleteOfficesAsync(empCode);
    }

    public async Task AddPostAsync(EmployeePost entity)
        => await _db.Set<EmployeePost>().AddAsync(entity);

    public async Task DeletePostsAsync(string empCode)
    {
        var list = await _db.Set<EmployeePost>().Where(e => e.EmpCode == empCode).ToListAsync();
        _db.Set<EmployeePost>().RemoveRange(list);
    }

    public async Task AddOfficeAsync(EmployeeOffice entity)
        => await _db.Set<EmployeeOffice>().AddAsync(entity);

    public async Task DeleteOfficesAsync(string empCode)
    {
        var list = await _db.Set<EmployeeOffice>().Where(e => e.EmpCode == empCode).ToListAsync();
        _db.Set<EmployeeOffice>().RemoveRange(list);
    }

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
