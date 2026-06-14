    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
namespace JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义class MenuDataScope
// 定义类：MenuDataScope
public class MenuDataScope : DataEntity
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
    // 属性 RuleName
    // 属性：RuleName
    public string RuleName { get; set; } = string.Empty;
    // 属性 RuleType
    // 属性：RuleType
    public string RuleType { get; set; } = string.Empty;
    // 属性：RuleConfig
    public string? RuleConfig { get; set; }
}
