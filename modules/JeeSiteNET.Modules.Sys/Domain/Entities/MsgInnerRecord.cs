    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
namespace JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义class MsgInnerRecord
// 定义类：MsgInnerRecord
public class MsgInnerRecord : BaseEntity
{
    // 属性 Id
    // 属性：Id
    public string Id { get; set; } = string.Empty;
    // 属性 MsgInnerId
    // 属性：MsgInnerId
    public string MsgInnerId { get; set; } = string.Empty;
    // 属性 ReceiveUserCode
    // 属性：ReceiveUserCode
    public string ReceiveUserCode { get; set; } = string.Empty;
    // 属性 ReceiveUserName
    // 属性：ReceiveUserName
    public string ReceiveUserName { get; set; } = string.Empty;
    // 属性 ReadStatus
    // 属性：ReadStatus
    public string ReadStatus { get; set; } = "0";
    // 属性：ReadDate
    public DateTime? ReadDate { get; set; }
    // 属性：IsStar
    public string? IsStar { get; set; }
}
