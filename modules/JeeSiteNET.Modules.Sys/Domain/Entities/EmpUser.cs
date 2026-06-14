    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
namespace JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义class EmpUser
// 定义类：EmpUser
public class EmpUser : BaseEntity
{
    // 属性 EmpCode
    // 属性：EmpCode
    public string EmpCode { get; set; } = string.Empty;
    // 属性 UserCode
    // 属性：UserCode
    public string UserCode { get; set; } = string.Empty;
    // 属性：EmpName
    public string? EmpName { get; set; }
    // 属性：LoginCode
    public string? LoginCode { get; set; }
    // 属性：UserName
    public string? UserName { get; set; }
}
