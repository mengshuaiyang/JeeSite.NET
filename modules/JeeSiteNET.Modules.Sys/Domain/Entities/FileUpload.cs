using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class FileUpload : DataEntity
{
    public string Id { get; set; } = string.Empty;
    public string FileId { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public decimal? FileSort { get; set; }
    public string? BizKey { get; set; }
    public string? BizType { get; set; }
}
