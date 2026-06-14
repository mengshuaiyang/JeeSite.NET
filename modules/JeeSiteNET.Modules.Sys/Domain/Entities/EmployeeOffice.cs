    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
namespace JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义class EmployeeOffice
// 定义类：EmployeeOffice
public class EmployeeOffice : DataEntity
{
    // 属性 Id
    // 属性：Id
    public string Id { get; set; } = string.Empty;
    // 属性 EmpCode
    // 属性：EmpCode
    public string EmpCode { get; set; } = string.Empty;
    // 属性 OfficeCode
    // 属性：OfficeCode
    public string OfficeCode { get; set; } = string.Empty;
    // 属性：PostCode
    public string? PostCode { get; set; }
}
