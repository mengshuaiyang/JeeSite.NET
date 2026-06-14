namespace JeeSiteNET.Core.UEditor;

/// <summary>
/// UEditor 文件上传/列表的抽象存储接口。
/// 将富文本编辑器的文件操作与底层存储解耦，便于在本地文件、OSS、MinIO 等不同实现间切换。
/// 默认实现可调用项目内 FileService 或直接落盘。
/// </summary>
public interface IUEditorUploadStore
{
    /// <summary>
    /// 保存上传文件流，返回可公开访问的 URL 与可选相对路径
    /// </summary>
    /// <param name="relativePath">目标相对路径（由 UEditor 路径格式化器生成）</param>
    /// <param name="stream">待保存的文件流</param>
    /// <param name="contentType">Content-Type</param>
    /// <param name="size">文件大小（字节）</param>
    /// <returns>包含 URL 与相对路径的保存结果</returns>
    Task<UEditorStoreResult> StoreAsync(string relativePath, Stream stream, string contentType, long size);

    /// <summary>
    /// 分页列出指定基础路径下的文件（用于 listimage / listfile 操作）
    /// </summary>
    /// <param name="basePath">目录相对路径</param>
    /// <param name="start">起始偏移</param>
    /// <param name="size">单次返回数量</param>
    /// <param name="allowedExtensions">允许的扩展名集合</param>
    /// <returns>文件条目列表</returns>
    Task<List<UEditorListEntry>> ListAsync(string basePath, int start, int size, string[] allowedExtensions);
}

/// <summary>
/// UEditor 文件保存结果
/// </summary>
public record UEditorStoreResult
{
    /// <summary>
    /// 可公开访问的文件 URL
    /// </summary>
    public string Url { get; init; } = string.Empty;

    /// <summary>
    /// 相对路径（可选，用于再次定位）
    /// </summary>
    public string? RelativePath { get; init; }
}
