using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly JeeSiteDbContext _db;

    public UserRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<User> Query() => _db.Set<User>().AsNoTracking();

    public async Task<User?> GetAsync(object id)
        => await _db.Set<User>().FindAsync(id);

    public async Task<List<User>> FindListAsync()
        => await _db.Set<User>().AsNoTracking().ToListAsync();

    public async Task AddAsync(User entity)
    {
        await _db.Set<User>().AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(User entity)
    {
        _db.Set<User>().Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(User entity)
    {
        _db.Set<User>().Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<User?> GetByLoginCodeAsync(string loginCode)
        => await _db.Set<User>().AsNoTracking()
            .FirstOrDefaultAsync(u => u.LoginCode == loginCode);

    public async Task<User?> GetByPhoneAsync(string phone)
        => await _db.Set<User>().AsNoTracking()
            .FirstOrDefaultAsync(u => u.Phone != null && u.Phone == phone);

    public async Task<User?> GetByEmailAsync(string email)
        => await _db.Set<User>().AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email != null && u.Email == email);
}
