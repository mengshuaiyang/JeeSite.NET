using JeeSiteNET.Core;
using JeeSiteNET.Modules.CodeGen.Domain.Entities;

namespace JeeSiteNET.Modules.CodeGen.Domain.Interfaces;

public interface IGenTableColumnRepository : IRepository<GenTableColumn>
{
    Task<List<GenTableColumn>> FindByTableNameAsync(string tableName);
}
