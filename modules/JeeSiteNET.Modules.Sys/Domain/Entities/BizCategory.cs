    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
namespace JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义class BizCategory
// 定义类：BizCategory
public class BizCategory : TreeEntity, ICorpEntity
{
    // 属性 CategoryCode
    // 属性：CategoryCode
    public string CategoryCode { get; set; } = string.Empty;
    // 属性：ViewCode
    public string? ViewCode { get; set; }
    // 属性 CategoryName
    // 属性：CategoryName
    public string CategoryName { get; set; } = string.Empty;

    // 属性：CorpCode
    public string? CorpCode { get; set; }
    // 属性：CorpName
    public string? CorpName { get; set; }

    // 方法：GetName
    public override string GetName() => CategoryName;
}
