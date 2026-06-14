    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Storage 命名空间
// 引入命名空间：JeeSiteNET.Core.Storage
using JeeSiteNET.Core.Storage;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
    // 引入 SkiaSharp 命名空间
// 引入命名空间：SkiaSharp
using SkiaSharp;

// 定义 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Application.Services
namespace JeeSiteNET.Modules.Sys.Application.Services;

// 定义class PreviewService
// 定义类：PreviewService
public class PreviewService
{
    // 字段 _fileUploadRepo
    // 字段：_fileUploadRepo
    private readonly IFileUploadRepository _fileUploadRepo;
    // 字段 _fileEntityRepo
    // 字段：_fileEntityRepo
    private readonly IFileEntityRepository _fileEntityRepo;
    // 字段 _storage
    // 字段：_storage
    private readonly IFileStorageProvider _storage;

    // 构造函数 PreviewService
    // 构造函数：PreviewService
    public PreviewService(
        IFileUploadRepository fileUploadRepo,
        IFileEntityRepository fileEntityRepo,
        IFileStorageProvider storage)
    {
        _fileUploadRepo = fileUploadRepo;
        _fileEntityRepo = fileEntityRepo;
        _storage = storage;
    }

    // 方法 GetPreviewAsync
    // 方法：GetPreviewAsync
    public async Task<PreviewResult?> GetPreviewAsync(string uploadId)
    {
        // 缓存：获取值
        var upload = await _fileUploadRepo.GetAsync(uploadId);
        // if 条件判断
        if (upload == null) return null;

        // 缓存：获取值
        var fe = await _fileEntityRepo.GetAsync(upload.FileId);
        // if 条件判断
        if (fe == null) return null;

        // 缓存：获取值
        var stream = await _storage.GetAsync(fe.FilePath);
        // if 条件判断
        if (stream == null) return null;

        // return 返回结果
        return new PreviewResult
        {
            Stream = stream,
            ContentType = GetPreviewContentType(upload.FileName, fe.FileContentType),
            FileName = upload.FileName,
        };
    }

    // 方法 GetThumbnailAsync
    // 方法：GetThumbnailAsync
    public async Task<PreviewResult?> GetThumbnailAsync(string uploadId, int width = 200, int height = 200)
    {
        // 缓存：获取值
        var upload = await _fileUploadRepo.GetAsync(uploadId);
        // if 条件判断
        if (upload == null) return null;

        // 缓存：获取值
        var fe = await _fileEntityRepo.GetAsync(upload.FileId);
        // if 条件判断
        if (fe == null) return null;

        // if 条件判断
        if (!IsImage(fe.FileExtension))
            // return 返回结果
            return await GetFileIcon(fe.FileExtension);

        // 缓存：获取值
        var stream = await _storage.GetAsync(fe.FilePath);
        // if 条件判断
        if (stream == null) return null;

    // 引入 var input 命名空间
        using var input = SKBitmap.Decode(stream);
        // if 条件判断
        if (input == null) return null;

        // 调用 Min
        var ratio = Math.Min((float)width / input.Width, (float)height / input.Height);
        // if 条件判断
        if (ratio >= 1) ratio = 1;

        // 声明并初始化变量：newW
        var newW = (int)(input.Width * ratio);
        // 声明并初始化变量：newH
        var newH = (int)(input.Height * ratio);

    // 引入 var resized 命名空间
        using var resized = input.Resize(new SKImageInfo(newW, newH), new SKSamplingOptions(SKFilterMode.Linear));
        // if 条件判断
        if (resized == null) return null;

    // 引入 var image 命名空间
        using var image = SKImage.FromBitmap(resized);
    // 引入 var data 命名空间
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, 80);
        // 创建 MemoryStream实例并赋给 ms
        var ms = new MemoryStream(data.ToArray());

        // return 返回结果
        return new PreviewResult
        {
            Stream = ms,
            ContentType = "image/jpeg",
            FileName = Path.GetFileNameWithoutExtension(upload.FileName) + "_thumb.jpg",
        };
    }

    // 方法 ConvertToPdfAsync
    // 方法：ConvertToPdfAsync
    public async Task<PreviewResult?> ConvertToPdfAsync(string uploadId)
    {
        // 缓存：获取值
        var upload = await _fileUploadRepo.GetAsync(uploadId);
        // if 条件判断
        if (upload == null) return null;

        // 缓存：获取值
        var fe = await _fileEntityRepo.GetAsync(upload.FileId);
        // if 条件判断
        if (fe == null) return null;

        // if 条件判断
        if (!IsOfficeDoc(fe.FileExtension))
            // return 返回结果
            return null;

        // 缓存：获取值
        var stream = await _storage.GetAsync(fe.FilePath);
        // if 条件判断
        if (stream == null) return null;

        // 声明并初始化变量：tempDir
        var tempDir = Path.Combine(Path.GetTempPath(), "jeesite_preview");
        Directory.CreateDirectory(tempDir);

        // 声明并初始化变量：tempInput
        var tempInput = Path.Combine(tempDir, upload.FileName);
        // await 异步等待
        await using (var fs = new FileStream(tempInput, FileMode.Create))
            // await 异步等待
            await stream.CopyToAsync(fs);

        // try 异常捕获开始
        try
        {
            var pdfPath = await LibreOfficeConverter.ConvertToPdfAsync(tempInput);
            // if 条件判断
            if (!File.Exists(pdfPath))
                // return 返回结果
                return null;

            // 创建 MemoryStream实例并赋给 ms
            var ms = new MemoryStream(await File.ReadAllBytesAsync(pdfPath));
            // return 返回结果
            return new PreviewResult
            {
                Stream = ms,
                ContentType = "application/pdf",
                FileName = Path.GetFileNameWithoutExtension(upload.FileName) + ".pdf",
            };
        }
        // finally 最终执行块
        finally
        {
            // if 条件判断
            if (File.Exists(tempInput)) File.Delete(tempInput);
        }
    }

    // 方法：IsImage
    private static bool IsImage(string ext) => ext switch
    {
        ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" or ".webp" or ".svg" => true,
        _ => false,
    };

    // 方法：IsOfficeDoc
    private static bool IsOfficeDoc(string ext) => ext switch
    {
        ".doc" or ".docx" or ".xls" or ".xlsx" or ".ppt" or ".pptx" or ".odt" or ".ods" or ".odp" => true,
        _ => false,
    };

    // 方法 GetPreviewContentType
    // 方法：GetPreviewContentType
    private static string GetPreviewContentType(string fileName, string defaultContentType)
    {
        // 声明并初始化变量：ext
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        // return 返回结果
        return ext switch
        {
            ".pdf" => "application/pdf",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            ".svg" => "image/svg+xml",
            ".mp4" => "video/mp4",
            ".mp3" => "audio/mpeg",
            _ => defaultContentType,
        };
    }

    // 方法 GetFileIcon
    // 方法：GetFileIcon
    private static Task<PreviewResult?> GetFileIcon(string ext)
    {
        // 创建 MemoryStream实例并赋给 ms
        var ms = new MemoryStream();
    // 引入 var bitmap 命名空间
        using var bitmap = new SKBitmap(64, 64);
    // 引入 var canvas 命名空间
        using var canvas = new SKCanvas(bitmap);
        // 集合操作：清空集合
        canvas.Clear(new SKColor(240, 240, 240));

    // 引入 var paint 命名空间
        using var paint = new SKPaint
        {
            // 创建 SKColor实例并赋给 Color
            Color = new SKColor(100, 100, 100),
            IsAntialias = true,
        };
    // 引入 var font 命名空间
        using var font = new SKFont(SKTypeface.Default, 20);
        // 声明并初始化变量：label
        var label = ext.TrimStart('.').ToUpperInvariant();
        canvas.DrawText(label, 32, 40, SKTextAlign.Center, font, paint);

    // 引入 var image 命名空间
        using var image = SKImage.FromBitmap(bitmap);
    // 引入 var data 命名空间
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        // 文件/流操作：写入
        ms.Write(data.ToArray());
        ms.Position = 0;

        // return 返回结果
        return Task.FromResult<PreviewResult?>(new PreviewResult
        {
            Stream = ms,
            ContentType = "image/png",
            FileName = $"icon_{label}.png",
        });
    }
}

// 定义class PreviewResult
// 定义类：PreviewResult
public class PreviewResult
{
    // 属性 Stream
    // 属性：Stream
    public Stream Stream { get; set; } = Stream.Null;
    // 属性 ContentType
    // 属性：ContentType
    public string ContentType { get; set; } = "application/octet-stream";
    // 属性 FileName
    // 属性：FileName
    public string FileName { get; set; } = string.Empty;
}
