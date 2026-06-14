    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
namespace JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义class UserDataScope
// 定义类：UserDataScope
public class UserDataScope : DataEntity
{
    // 属性 UserCode
    // 属性：UserCode
    public string UserCode { get; set; } = string.Empty;
    // 属性 CtrlType
    // 属性：CtrlType
    public string CtrlType { get; set; } = string.Empty;
    // 属性：CtrlData
    public string? CtrlData { get; set; }
    // 属性：CtrlPermi
    public string? CtrlPermi { get; set; }
}
