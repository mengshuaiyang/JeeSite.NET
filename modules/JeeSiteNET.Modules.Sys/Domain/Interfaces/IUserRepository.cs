using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByLoginCodeAsync(string loginCode);
}
