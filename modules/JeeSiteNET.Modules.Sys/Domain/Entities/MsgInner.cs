using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class MsgInner : DataEntity
{
    public string Id { get; set; } = string.Empty;
    public string MsgTitle { get; set; } = string.Empty;
    public string ContentLevel { get; set; } = "1";
    public string? ContentType { get; set; }
    public string MsgContent { get; set; } = string.Empty;
    public string ReceiveType { get; set; } = "user";
    public string? ReceiveCodes { get; set; }
    public string? ReceiveNames { get; set; }
    public string? SendUserCode { get; set; }
    public string? SendUserName { get; set; }
    public DateTime? SendDate { get; set; }
    public string? IsAttac { get; set; }
    public string? NotifyTypes { get; set; }
}
