    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.App.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.App.Domain.Entities
namespace JeeSiteNET.Modules.App.Domain.Entities;

// 定义class AppComment
// 定义类：AppComment
public class AppComment : DataEntity
{
    // 属性 Id
    // 属性：Id
    public string Id { get; set; } = string.Empty;
    // 属性：Category
    public string? Category { get; set; }
    // 属性：Content
    public string? Content { get; set; }
    // 属性：Contact
    public string? Contact { get; set; }
    // 属性：CreateByName
    public string? CreateByName { get; set; }
    // 属性：DeviceInfo
    public string? DeviceInfo { get; set; }
    // 属性：ReplyDate
    public DateTime? ReplyDate { get; set; }
    // 属性：ReplyContent
    public string? ReplyContent { get; set; }
    // 属性：ReplyUserCode
    public string? ReplyUserCode { get; set; }
    // 属性：ReplyUserName
    public string? ReplyUserName { get; set; }
}
