using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class MsgInnerRecord : BaseEntity
{
    public string Id { get; set; } = string.Empty;
    public string MsgInnerId { get; set; } = string.Empty;
    public string ReceiveUserCode { get; set; } = string.Empty;
    public string ReceiveUserName { get; set; } = string.Empty;
    public string ReadStatus { get; set; } = "0";
    public DateTime? ReadDate { get; set; }
    public string? IsStar { get; set; }
}
