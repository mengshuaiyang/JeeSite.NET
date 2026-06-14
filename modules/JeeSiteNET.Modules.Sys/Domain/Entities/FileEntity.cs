namespace JeeSiteNET.Modules.Sys.Domain.Entities;

/// <summary>
/// 文件实体，代表一份物理存储的文件。基于 MD5 去重，多文件上传可引用同一份物理文件。
/// </summary>
public class FileEntity
{
    /// <summary>文件 ID，业务主键。</summary>
    public string FileId { get; set; } = string.Empty;
    /// <summary>文件 MD5 哈希值（用于文件秒传与去重）。</summary>
    public string FileMd5 { get; set; } = string.Empty;
    /// <summary>文件在存储服务中的路径或 Key（MinIO / OSS / 本地路径）。</summary>
    public string FilePath { get; set; } = string.Empty;
    /// <summary>文件 MIME 类型（如 image/png、application/pdf）。</summary>
    public string FileContentType { get; set; } = string.Empty;
    /// <summary>文件扩展名（不含点号，如 png、docx）。</summary>
    public string FileExtension { get; set; } = string.Empty;
    /// <summary>文件大小（字节）。</summary>
    public decimal FileSize { get; set; }
    /// <summary>文件元数据（JSON 格式，如图片宽高、视频时长、文档页数）。</summary>
    public string? FileMeta { get; set; }
    /// <summary>文件预览 URL（如缩略图、文档预览地址）。</summary>
    public string? FilePreview { get; set; }
}
