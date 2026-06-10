using JeeSiteNET.Core.Storage;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;

namespace JeeSiteNET.Infrastructure.FileStorage;

public class S3StorageProvider : IFileStorageProvider
{
    private readonly IMinioClient _client;
    private readonly string _bucket;
    private readonly ILogger<S3StorageProvider> _logger;

    public S3StorageProvider(S3Options options, ILogger<S3StorageProvider> logger)
    {
        _bucket = options.Bucket;
        _logger = logger;

        _client = new MinioClient()
            .WithEndpoint(options.Endpoint)
            .WithCredentials(options.AccessKey, options.SecretKey)
            .WithSSL(options.UseSsl)
            .WithRegion(options.Region)
            .Build();
    }

    public string Name => "s3";

    public async Task<string> SaveAsync(string fileId, Stream content, string extension)
    {
        var bucketExists = await _client.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucket));
        if (!bucketExists)
            await _client.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucket));

        var datePath = DateTime.Now.ToString("yyyy/MM/dd");
        var objectName = $"{datePath}/{fileId}{extension}";

        content.Position = 0;
        await _client.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_bucket)
            .WithObject(objectName)
            .WithStreamData(content)
            .WithObjectSize(content.Length));

        _logger.LogInformation("Uploaded {Object} to bucket {Bucket}", objectName, _bucket);
        return objectName;
    }

    public async Task<Stream?> GetAsync(string filePath)
    {
        var ms = new MemoryStream();
        var args = new GetObjectArgs()
            .WithBucket(_bucket)
            .WithObject(filePath)
            .WithCallbackStream(s => s.CopyTo(ms));

        try
        {
            await _client.GetObjectAsync(args);
            ms.Position = 0;
            return ms;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get {Object} from bucket {Bucket}", filePath, _bucket);
            return null;
        }
    }

    public async Task DeleteAsync(string filePath)
    {
        await _client.RemoveObjectAsync(new RemoveObjectArgs()
            .WithBucket(_bucket)
            .WithObject(filePath));
    }

    public string GetUrl(string filePath)
    {
        try
        {
            var task = _client.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket(_bucket)
                .WithObject(filePath)
                .WithExpiry(3600));
            return task.GetAwaiter().GetResult();
        }
        catch
        {
            return $"/api/v1/sys/file/download/by-path?path={Uri.EscapeDataString(filePath)}";
        }
    }
}
