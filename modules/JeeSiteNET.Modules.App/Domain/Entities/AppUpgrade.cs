using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.App.Domain.Entities;

public class AppUpgrade : DataEntity
{
    public string Id { get; set; } = string.Empty;
    public string? AppCode { get; set; }
    public string? UpTitle { get; set; }
    public string? UpContent { get; set; }
    public int? UpVersion { get; set; }
    public string? UpType { get; set; }
    public DateTime? UpDate { get; set; }
    public string? ApkUrl { get; set; }
    public string? ResUrl { get; set; }
}
