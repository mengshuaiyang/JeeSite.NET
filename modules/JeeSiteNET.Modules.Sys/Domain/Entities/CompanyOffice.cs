    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
namespace JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义class CompanyOffice
// 定义类：CompanyOffice
public class CompanyOffice : DataEntity
{
    // 属性 Id
    // 属性：Id
    public string Id { get; set; } = string.Empty;
    // 属性 CompanyCode
    // 属性：CompanyCode
    public string CompanyCode { get; set; } = string.Empty;
    // 属性 OfficeCode
    // 属性：OfficeCode
    public string OfficeCode { get; set; } = string.Empty;
}
