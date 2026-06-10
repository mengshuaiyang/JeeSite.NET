namespace JeeSiteNET.Core.Storage;

public class StorageOptions
{
    public string Provider { get; set; } = "local";
    public S3Options? S3 { get; set; }
    public AliyunOssOptions? Aliyun { get; set; }
}

public class S3Options
{
    public string Endpoint { get; set; } = "localhost:9000";
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string Bucket { get; set; } = "files";
    public bool UseSsl { get; set; }
    public string Region { get; set; } = "us-east-1";
}

public class AliyunOssOptions
{
    public string Endpoint { get; set; } = "oss-cn-hangzhou.aliyuncs.com";
    public string AccessKeyId { get; set; } = string.Empty;
    public string AccessKeySecret { get; set; } = string.Empty;
    public string Bucket { get; set; } = "jeesite";
    public string Region { get; set; } = "cn-hangzhou";
}
