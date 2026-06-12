namespace JeeSiteNET.Core.UEditor;

/// <summary>
/// 上传存储抽象 — 使 UEditor 协议与实际存储解耦。
/// 默认实现会调用项目中已有的 FileService（即 FileUploadController 等价逻辑）。
/// 也可以替换为 MinIO / 阿里云 OSS 等。
/// </summary>
public interface IUEditorUploadStore
{
    /// <summary>保存文件流，返回可公开访问的 URL 与（可选）相对路径</summary>
    Task<UEditorStoreResult> StoreAsync(string relativePath, Stream stream, string contentType, long size);

    /// <summary>列出指定路径下的文件（用于 listimage/listfile）</summary>
    Task<List<UEditorListEntry>> ListAsync(string basePath, int start, int size, string[] allowedExtensions);
}

public record UEditorStoreResult
{
    public string Url { get; init; } = string.Empty;
    public string? RelativePath { get; init; }
}
