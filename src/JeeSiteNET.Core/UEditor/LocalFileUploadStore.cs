using System.Text;

namespace JeeSiteNET.Core.UEditor;

/// <summary>
/// 默认实现：将文件保存到 "wwwroot/{relativePath}" 并返回静态文件可访问 URL。
/// 与 FileService 一致的方式，便于后续用 FileService 替代。
/// </summary>
public sealed class LocalFileUploadStore : IUEditorUploadStore
{
    private readonly string _webRoot;
    private readonly string _baseUrl;

    public LocalFileUploadStore(string webRoot, string baseUrl = "")
    {
        _webRoot = webRoot ?? throw new ArgumentNullException(nameof(webRoot));
        _baseUrl = baseUrl ?? string.Empty;
    }

    public async Task<UEditorStoreResult> StoreAsync(string relativePath, Stream stream, string contentType, long size)
    {
        var fullPath = Path.Combine(_webRoot, relativePath.TrimStart('/', '\\'));
        var dir = Path.GetDirectoryName(fullPath)!;
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        using var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
        await stream.CopyToAsync(fs);
        await fs.FlushAsync();

        var url = _baseUrl + "/" + relativePath.Replace('\\', '/').TrimStart('/');
        return new UEditorStoreResult { Url = url, RelativePath = relativePath };
    }

    public Task<List<UEditorListEntry>> ListAsync(string basePath, int start, int size, string[] allowedExtensions)
    {
        var dir = Path.Combine(_webRoot, basePath.TrimStart('/', '\\'));
        var result = new List<UEditorListEntry>();
        if (!Directory.Exists(dir)) return Task.FromResult(result);

        var files = Directory.EnumerateFiles(dir, "*.*", SearchOption.AllDirectories)
            .Where(f => allowedExtensions.Contains(Path.GetExtension(f).ToLowerInvariant()))
            .Select(f => new { Path = f, Time = new FileInfo(f).LastWriteTime.ToUnixTimeSeconds() })
            .OrderByDescending(f => f.Time)
            .Skip(start)
            .Take(size)
            .ToList();

        foreach (var f in files)
        {
            var rel = f.Path[(_webRoot.Length + 1)..].Replace('\\', '/');
            result.Add(new UEditorListEntry { Url = _baseUrl + "/" + rel, Mtime = f.Time });
        }

        return Task.FromResult(result);
    }
}

/// <summary>
/// 日期时间扩展
/// </summary>
internal static class DateTimeExtensions
{
    public static long ToUnixTimeSeconds(this DateTime dt) =>
        (long)(dt.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
}
