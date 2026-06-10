using JeeSiteNET.Core;
using JeeSiteNET.Core.Storage;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using SkiaSharp;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class PreviewService
{
    private readonly IFileUploadRepository _fileUploadRepo;
    private readonly IFileEntityRepository _fileEntityRepo;
    private readonly IFileStorageProvider _storage;

    public PreviewService(
        IFileUploadRepository fileUploadRepo,
        IFileEntityRepository fileEntityRepo,
        IFileStorageProvider storage)
    {
        _fileUploadRepo = fileUploadRepo;
        _fileEntityRepo = fileEntityRepo;
        _storage = storage;
    }

    public async Task<PreviewResult?> GetPreviewAsync(string uploadId)
    {
        var upload = await _fileUploadRepo.GetAsync(uploadId);
        if (upload == null) return null;

        var fe = await _fileEntityRepo.GetAsync(upload.FileId);
        if (fe == null) return null;

        var stream = await _storage.GetAsync(fe.FilePath);
        if (stream == null) return null;

        return new PreviewResult
        {
            Stream = stream,
            ContentType = GetPreviewContentType(upload.FileName, fe.FileContentType),
            FileName = upload.FileName,
        };
    }

    public async Task<PreviewResult?> GetThumbnailAsync(string uploadId, int width = 200, int height = 200)
    {
        var upload = await _fileUploadRepo.GetAsync(uploadId);
        if (upload == null) return null;

        var fe = await _fileEntityRepo.GetAsync(upload.FileId);
        if (fe == null) return null;

        if (!IsImage(fe.FileExtension))
            return await GetFileIcon(fe.FileExtension);

        var stream = await _storage.GetAsync(fe.FilePath);
        if (stream == null) return null;

        using var input = SKBitmap.Decode(stream);
        if (input == null) return null;

        var ratio = Math.Min((float)width / input.Width, (float)height / input.Height);
        if (ratio >= 1) ratio = 1;

        var newW = (int)(input.Width * ratio);
        var newH = (int)(input.Height * ratio);

        using var resized = input.Resize(new SKImageInfo(newW, newH), new SKSamplingOptions(SKFilterMode.Linear));
        if (resized == null) return null;

        using var image = SKImage.FromBitmap(resized);
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, 80);
        var ms = new MemoryStream(data.ToArray());

        return new PreviewResult
        {
            Stream = ms,
            ContentType = "image/jpeg",
            FileName = Path.GetFileNameWithoutExtension(upload.FileName) + "_thumb.jpg",
        };
    }

    public async Task<PreviewResult?> ConvertToPdfAsync(string uploadId)
    {
        var upload = await _fileUploadRepo.GetAsync(uploadId);
        if (upload == null) return null;

        var fe = await _fileEntityRepo.GetAsync(upload.FileId);
        if (fe == null) return null;

        if (!IsOfficeDoc(fe.FileExtension))
            return null;

        var stream = await _storage.GetAsync(fe.FilePath);
        if (stream == null) return null;

        var tempDir = Path.Combine(Path.GetTempPath(), "jeesite_preview");
        Directory.CreateDirectory(tempDir);

        var tempInput = Path.Combine(tempDir, upload.FileName);
        await using (var fs = new FileStream(tempInput, FileMode.Create))
            await stream.CopyToAsync(fs);

        try
        {
            var pdfPath = await LibreOfficeConverter.ConvertToPdfAsync(tempInput);
            if (!File.Exists(pdfPath))
                return null;

            var ms = new MemoryStream(await File.ReadAllBytesAsync(pdfPath));
            return new PreviewResult
            {
                Stream = ms,
                ContentType = "application/pdf",
                FileName = Path.GetFileNameWithoutExtension(upload.FileName) + ".pdf",
            };
        }
        finally
        {
            if (File.Exists(tempInput)) File.Delete(tempInput);
        }
    }

    private static bool IsImage(string ext) => ext switch
    {
        ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" or ".webp" or ".svg" => true,
        _ => false,
    };

    private static bool IsOfficeDoc(string ext) => ext switch
    {
        ".doc" or ".docx" or ".xls" or ".xlsx" or ".ppt" or ".pptx" or ".odt" or ".ods" or ".odp" => true,
        _ => false,
    };

    private static string GetPreviewContentType(string fileName, string defaultContentType)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
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

    private static Task<PreviewResult?> GetFileIcon(string ext)
    {
        var ms = new MemoryStream();
        using var bitmap = new SKBitmap(64, 64);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(new SKColor(240, 240, 240));

        using var paint = new SKPaint
        {
            Color = new SKColor(100, 100, 100),
            IsAntialias = true,
        };
        using var font = new SKFont(SKTypeface.Default, 20);
        var label = ext.TrimStart('.').ToUpperInvariant();
        canvas.DrawText(label, 32, 40, SKTextAlign.Center, font, paint);

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        ms.Write(data.ToArray());
        ms.Position = 0;

        return Task.FromResult<PreviewResult?>(new PreviewResult
        {
            Stream = ms,
            ContentType = "image/png",
            FileName = $"icon_{label}.png",
        });
    }
}

public class PreviewResult
{
    public Stream Stream { get; set; } = Stream.Null;
    public string ContentType { get; set; } = "application/octet-stream";
    public string FileName { get; set; } = string.Empty;
}
