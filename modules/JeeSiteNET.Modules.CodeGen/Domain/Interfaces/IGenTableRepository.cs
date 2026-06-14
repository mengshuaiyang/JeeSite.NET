    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.CodeGen.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.CodeGen.Domain.Entities
using JeeSiteNET.Modules.CodeGen.Domain.Entities;

// 定义 JeeSiteNET.Modules.CodeGen.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.CodeGen.Domain.Interfaces
namespace JeeSiteNET.Modules.CodeGen.Domain.Interfaces;

// 定义接口 IGenTableRepository
// 定义接口：IGenTableRepository
public interface IGenTableRepository : IRepository<GenTable>
{
    Task<List<GenTable>> FindListWithColumnsAsync();
    Task<GenTable?> GetWithColumnsAsync(string tableName);
    Task<List<GenTableColumn>> GetColumnsAsync(string tableName);
}
