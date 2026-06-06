using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

public interface IDictDataRepository : IRepository<DictData>
{
    Task<List<DictData>> GetByTypeAsync(string dictType);
}
