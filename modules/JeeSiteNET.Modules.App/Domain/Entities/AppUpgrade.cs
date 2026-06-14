    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.App.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.App.Domain.Entities
namespace JeeSiteNET.Modules.App.Domain.Entities;

// 定义class AppUpgrade
// 定义类：AppUpgrade
public class AppUpgrade : DataEntity
{
    // 属性 Id
    // 属性：Id
    public string Id { get; set; } = string.Empty;
    // 属性：AppCode
    public string? AppCode { get; set; }
    // 属性：UpTitle
    public string? UpTitle { get; set; }
    // 属性：UpContent
    public string? UpContent { get; set; }
    // 属性：UpVersion
    public int? UpVersion { get; set; }
    // 属性：UpType
    public string? UpType { get; set; }
    // 属性：UpDate
    public DateTime? UpDate { get; set; }
    // 属性：ApkUrl
    public string? ApkUrl { get; set; }
    // 属性：ResUrl
    public string? ResUrl { get; set; }
}
