    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
namespace JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义class Area
// 定义类：Area
public class Area : TreeEntity
{
    // 属性 AreaCode
    // 属性：AreaCode
    public string AreaCode { get; set; } = string.Empty;
    // 属性 AreaName
    // 属性：AreaName
    public string AreaName { get; set; } = string.Empty;
    // 属性：AreaType
    public string? AreaType { get; set; }

    // 方法：GetName
    public override string GetName() => AreaName;
}
