    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.CodeGen.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.CodeGen.Domain.Entities
using JeeSiteNET.Modules.CodeGen.Domain.Entities;

// 定义 JeeSiteNET.Modules.CodeGen.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.CodeGen.Domain.Interfaces
namespace JeeSiteNET.Modules.CodeGen.Domain.Interfaces;

// 定义接口 IGenTableColumnRepository
// 定义接口：IGenTableColumnRepository
public interface IGenTableColumnRepository : IRepository<GenTableColumn>
{
    Task<List<GenTableColumn>> FindByTableNameAsync(string tableName);
}
