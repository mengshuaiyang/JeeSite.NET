namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class FileEntity
{
    public string FileId { get; set; } = string.Empty;
    public string FileMd5 { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string FileContentType { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
    public decimal FileSize { get; set; }
    public string? FileMeta { get; set; }
    public string? FilePreview { get; set; }
}
