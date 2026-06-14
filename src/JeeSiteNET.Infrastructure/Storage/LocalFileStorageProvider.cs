using JeeSiteNET.Core.Storage;

namespace JeeSiteNET.Infrastructure.Storage;

/// <summary>
/// 本地文件系统存储提供者。
/// 将文件按日期分目录存储在配置的基础路径下，并通过基础 URL 拼接对外提供访问路径。
/// </summary>
public class LocalFileStorageProvider : IFileStorageProvider
{
    private readonly string _basePath;
    private readonly string _baseUrl;

    /// <summary>
    /// 初始化 <see cref="LocalFileStorageProvider"/> 的新实例。
    /// </summary>
    /// <param name="basePath">本地存储根目录（相对或绝对路径）；若不存在会自动创建。</param>
    /// <param name="baseUrl">对外访问 URL 前缀，用于拼接下载地址。</param>
    public LocalFileStorageProvider(string basePath, string baseUrl)
    {
        _basePath = basePath;
        _baseUrl = baseUrl.TrimEnd('/');
        if (!Directory.Exists(_basePath))
            Directory.CreateDirectory(_basePath);
    }

    /// <summary>
    /// 获取提供者名称标识。
    /// </summary>
    public string Name => "local";

    /// <summary>
    /// 将文件流保存到本地，按日期分目录（yyyy/MM/dd/fileId.ext）。
    /// </summary>
    /// <param name="fileId">文件唯一标识。</param>
    /// <param name="content">文件流。</param>
    /// <param name="extension">扩展名（带点）。</param>
    /// <returns>相对路径，使用斜杠分隔。</returns>
    public Task<string> SaveAsync(string fileId, Stream content, string extension)
    {
        var datePath = DateTime.Now.ToString("yyyy/MM/dd");
        var dir = Path.Combine(_basePath, datePath);
        Directory.CreateDirectory(dir);
        var fileName = $"{fileId}{extension}";
        var filePath = Path.Combine(dir, fileName);

        using var fs = new FileStream(filePath, FileMode.Create);
        content.CopyTo(fs);

        var relativePath = Path.Combine(datePath, fileName).Replace('\\', '/');
        return Task.FromResult(relativePath);
    }

    /// <summary>
    /// 读取本地文件流；文件不存在时返回 null。
    /// </summary>
    /// <param name="filePath">相对路径（日期 + 文件名）。</param>
    /// <returns>只读文件流。</returns>
    public async Task<Stream?> GetAsync(string filePath)
    {
        var fullPath = Path.Combine(_basePath, filePath);
        if (!File.Exists(fullPath)) return null;
        return new FileStream(fullPath, FileMode.Open, FileAccess.Read);
    }

    /// <summary>
    /// 删除本地文件；文件不存在时静默完成。
    /// </summary>
    /// <param name="filePath">相对路径。</param>
    /// <returns>表示异步操作的任务。</returns>
    public Task DeleteAsync(string filePath)
    {
        var fullPath = Path.Combine(_basePath, filePath);
        if (File.Exists(fullPath)) File.Delete(fullPath);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 拼接基础 URL 与相对路径形成可访问的外部 URL。
    /// </summary>
    /// <param name="filePath">相对路径。</param>
    /// <returns>完整 URL 字符串。</returns>
    public string GetUrl(string filePath) => $"{_baseUrl}/{filePath.Replace('\\', '/')}";
}
