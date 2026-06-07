using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.App.Domain.Entities;

public class AppComment : DataEntity
{
    public string Id { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? Content { get; set; }
    public string? Contact { get; set; }
    public string? CreateByName { get; set; }
    public string? DeviceInfo { get; set; }
    public DateTime? ReplyDate { get; set; }
    public string? ReplyContent { get; set; }
    public string? ReplyUserCode { get; set; }
    public string? ReplyUserName { get; set; }
}
