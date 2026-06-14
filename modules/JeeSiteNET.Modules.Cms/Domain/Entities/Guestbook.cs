    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
namespace JeeSiteNET.Modules.Cms.Domain.Entities;

// 定义class Guestbook
// 定义类：Guestbook
public class Guestbook : DataEntity, ICorpEntity
{
    // 属性 GbCode
    // 属性：GbCode
    public string GbCode { get; set; } = string.Empty;
    // 属性 GbType
    // 属性：GbType
    public string GbType { get; set; } = string.Empty;
    // 属性 Content
    // 属性：Content
    public string Content { get; set; } = string.Empty;
    // 属性 Name
    // 属性：Name
    public string Name { get; set; } = string.Empty;
    // 属性 Email
    // 属性：Email
    public string Email { get; set; } = string.Empty;
    // 属性 Phone
    // 属性：Phone
    public string Phone { get; set; } = string.Empty;
    // 属性 WorkUnit
    // 属性：WorkUnit
    public string WorkUnit { get; set; } = string.Empty;
    // 属性 Ip
    // 属性：Ip
    public string Ip { get; set; } = string.Empty;
    // 属性：ReUserCode
    public string? ReUserCode { get; set; }
    // 属性：ReDate
    public DateTime? ReDate { get; set; }
    // 属性：ReContent
    public string? ReContent { get; set; }

    // 属性：CorpCode
    public string? CorpCode { get; set; }
    // 属性：CorpName
    public string? CorpName { get; set; }
}
