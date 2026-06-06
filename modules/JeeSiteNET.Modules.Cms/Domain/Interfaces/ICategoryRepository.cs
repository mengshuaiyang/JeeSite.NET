using JeeSiteNET.Core;
using JeeSiteNET.Modules.Cms.Domain.Entities;

namespace JeeSiteNET.Modules.Cms.Domain.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<List<Category>> FindTreeAsync();
}
