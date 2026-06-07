using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Cms.Domain.Entities;

public class Guestbook : DataEntity, ICorpEntity
{
    public string GbCode { get; set; } = string.Empty;
    public string GbType { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string WorkUnit { get; set; } = string.Empty;
    public string Ip { get; set; } = string.Empty;
    public string? ReUserCode { get; set; }
    public DateTime? ReDate { get; set; }
    public string? ReContent { get; set; }

    public string? CorpCode { get; set; }
    public string? CorpName { get; set; }
}
