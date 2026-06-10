using Aliyun.OSS;
using JeeSiteNET.Core.Storage;
using Microsoft.Extensions.Logging;

namespace JeeSiteNET.Infrastructure.FileStorage;

public class AliyunOssStorageProvider : IFileStorageProvider
{
    private readonly OssClient _client;
    private readonly string _bucket;
    private readonly ILogger<AliyunOssStorageProvider> _logger;

    public AliyunOssStorageProvider(AliyunOssOptions options, ILogger<AliyunOssStorageProvider> logger)
    {
        _bucket = options.Bucket;
        _logger = logger;
        _client = new OssClient(options.Endpoint, options.AccessKeyId, options.AccessKeySecret);
    }

    public string Name => "aliyun";

    public Task<string> SaveAsync(string fileId, Stream content, string extension)
    {
        var datePath = DateTime.Now.ToString("yyyy/MM/dd");
        var objectName = $"{datePath}/{fileId}{extension}";
        content.Position = 0;

        var result = _client.PutObject(_bucket, objectName, content);
        _logger.LogInformation("Uploaded {Object} to Aliyun OSS bucket {Bucket}, ETag: {ETag}",
            objectName, _bucket, result.ETag);
        return Task.FromResult(objectName);
    }

    public async Task<Stream?> GetAsync(string filePath)
    {
        try
        {
            var obj = _client.GetObject(_bucket, filePath);
            var ms = new MemoryStream();
            await obj.Content.CopyToAsync(ms);
            ms.Position = 0;
            return ms;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get {Object} from Aliyun OSS bucket {Bucket}", filePath, _bucket);
            return null;
        }
    }

    public Task DeleteAsync(string filePath)
    {
        _client.DeleteObject(_bucket, filePath);
        return Task.CompletedTask;
    }

    public string GetUrl(string filePath)
    {
        var req = new GeneratePresignedUriRequest(_bucket, filePath, SignHttpMethod.Get)
        {
            Expiration = DateTime.UtcNow.AddHours(1)
        };
        return _client.GeneratePresignedUri(req).ToString();
    }
}
