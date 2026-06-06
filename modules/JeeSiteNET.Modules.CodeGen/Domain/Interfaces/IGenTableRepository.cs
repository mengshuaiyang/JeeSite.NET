using JeeSiteNET.Core;
using JeeSiteNET.Modules.CodeGen.Domain.Entities;

namespace JeeSiteNET.Modules.CodeGen.Domain.Interfaces;

public interface IGenTableRepository : IRepository<GenTable>
{
    Task<List<GenTable>> FindListWithColumnsAsync();
    Task<GenTable?> GetWithColumnsAsync(string tableName);
    Task<List<GenTableColumn>> GetColumnsAsync(string tableName);
}
