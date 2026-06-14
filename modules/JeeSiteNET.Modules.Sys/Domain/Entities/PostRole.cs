    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
namespace JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义class PostRole
// 定义类：PostRole
public class PostRole : DataEntity
{
    // 属性 PostCode
    // 属性：PostCode
    public string PostCode { get; set; } = string.Empty;
    // 属性 RoleCode
    // 属性：RoleCode
    public string RoleCode { get; set; } = string.Empty;
}
