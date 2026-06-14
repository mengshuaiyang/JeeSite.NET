    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
namespace JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义class Lang
// 定义类：Lang
public class Lang : BaseEntity
{
    // 属性 Id
    // 属性：Id
    public string Id { get; set; } = string.Empty;
    // 属性 ModuleCode
    // 属性：ModuleCode
    public string ModuleCode { get; set; } = string.Empty;
    // 属性 LangCode
    // 属性：LangCode
    public string LangCode { get; set; } = string.Empty;
    // 属性 LangText
    // 属性：LangText
    public string LangText { get; set; } = string.Empty;
    // 属性 LangType
    // 属性：LangType
    public string LangType { get; set; } = string.Empty;
}
