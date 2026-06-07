using JeeSiteNET.Core.Storage;

namespace JeeSiteNET.Infrastructure.Storage;

public class LocalFileStorageProvider : IFileStorageProvider
{
    private readonly string _basePath;
    private readonly string _baseUrl;

    public LocalFileStorageProvider(string basePath, string baseUrl)
    {
        _basePath = basePath;
        _baseUrl = baseUrl.TrimEnd('/');
        if (!Directory.Exists(_basePath))
            Directory.CreateDirectory(_basePath);
    }

    public string Name => "local";

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

    public async Task<Stream?> GetAsync(string filePath)
    {
        var fullPath = Path.Combine(_basePath, filePath);
        if (!File.Exists(fullPath)) return null;
        return new FileStream(fullPath, FileMode.Open, FileAccess.Read);
    }

    public Task DeleteAsync(string filePath)
    {
        var fullPath = Path.Combine(_basePath, filePath);
        if (File.Exists(fullPath)) File.Delete(fullPath);
        return Task.CompletedTask;
    }

    public string GetUrl(string filePath) => $"{_baseUrl}/{filePath.Replace('\\', '/')}";
}
