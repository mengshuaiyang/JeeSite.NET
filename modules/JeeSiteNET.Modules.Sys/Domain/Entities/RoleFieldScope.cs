    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
namespace JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义class RoleFieldScope
// 定义类：RoleFieldScope
public class RoleFieldScope : DataEntity
{
    // 属性 Id
    // 属性：Id
    public string Id { get; set; } = string.Empty;
    // 属性 RoleCode
    // 属性：RoleCode
    public string RoleCode { get; set; } = string.Empty;
    // 属性 MenuCode
    // 属性：MenuCode
    public string MenuCode { get; set; } = string.Empty;
    // 属性 EntityName
    // 属性：EntityName
    public string EntityName { get; set; } = string.Empty;
    // 属性 EntityLabel
    // 属性：EntityLabel
    public string EntityLabel { get; set; } = string.Empty;
    // 属性 EntityClass
    // 属性：EntityClass
    public string EntityClass { get; set; } = string.Empty;
    // 属性：FieldConfig
    public string? FieldConfig { get; set; }
}
