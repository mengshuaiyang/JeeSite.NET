    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
namespace JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义class EmployeePost
// 定义类：EmployeePost
public class EmployeePost : DataEntity
{
    // 属性 EmpCode
    // 属性：EmpCode
    public string EmpCode { get; set; } = string.Empty;
    // 属性 PostCode
    // 属性：PostCode
    public string PostCode { get; set; } = string.Empty;
}
