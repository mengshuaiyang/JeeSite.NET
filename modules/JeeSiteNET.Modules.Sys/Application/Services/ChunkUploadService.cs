using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class ChunkInfo
{
    public string UploadId { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long TotalSize { get; set; }
    public long ChunkSize { get; set; }
    public int TotalChunks { get; set; }
    public string? FileMd5 { get; set; }
    public string? BizType { get; set; }
    public string? BizKey { get; set; }
    public HashSet<int> ReceivedChunks { get; set; } = [];
    public DateTime CreateTime { get; set; } = DateTime.Now;
}

public class ChunkInitRequest
{
    public string FileName { get; set; } = string.Empty;
    public long TotalSize { get; set; }
    public long ChunkSize { get; set; }
    public int TotalChunks { get; set; }
    public string? FileMd5 { get; set; }
    public string? BizType { get; set; }
    public string? BizKey { get; set; }
}

public class ChunkInitResult
{
    public string UploadId { get; set; } = string.Empty;
    public bool Exist { get; set; }
    public string? FileUrl { get; set; }
    public string? UploadId2 { get; set; }
}

public class ChunkStatusResult
{
    public string UploadId { get; set; } = string.Empty;
    public List<int> ReceivedChunks { get; set; } = [];
    public bool IsCompleted { get; set; }
}

public class ChunkUploadService
{
    private static readonly ConcurrentDictionary<string, ChunkInfo> ChunkStore = new();
    private readonly FileService _fileService;
    private readonly string _tempDir;

    public ChunkUploadService(FileService fileService)
    {
        _fileService = fileService;
        _tempDir = Path.Combine(Path.GetTempPath(), "jeesite-chunks");
        Directory.CreateDirectory(_tempDir);
    }

    public async Task<ChunkInitResult> InitAsync(ChunkInitRequest request)
    {
        if (!string.IsNullOrEmpty(request.FileMd5))
        {
            var existResult = await _fileService.CheckMd5ExistAsync(request.FileMd5);
            if (existResult != null)
            {
                return new ChunkInitResult { UploadId = existResult.UploadId, Exist = true, FileUrl = existResult.FileUrl, UploadId2 = existResult.UploadId2 };
            }
        }

        var uploadId = IdGenerator.NewId();
        var info = new ChunkInfo
        {
            UploadId = uploadId,
            FileName = request.FileName,
            TotalSize = request.TotalSize,
            ChunkSize = request.ChunkSize,
            TotalChunks = request.TotalChunks,
            FileMd5 = request.FileMd5,
            BizType = request.BizType,
            BizKey = request.BizKey
        };

        var dir = GetChunkDir(uploadId);
        Directory.CreateDirectory(dir);

        ChunkStore[uploadId] = info;
        SaveMetadata(info);

        return new ChunkInitResult { UploadId = uploadId, Exist = false };
    }

    public async Task<ApiResult> UploadChunkAsync(string uploadId, int chunkIndex, IFormFile file)
    {
        if (!ChunkStore.TryGetValue(uploadId, out var info))
            return ApiResult.Fail(404, "上传会话不存在");

        var chunkDir = GetChunkDir(uploadId);
        var chunkPath = Path.Combine(chunkDir, $"chunk_{chunkIndex:D6}");

        await using var stream = new FileStream(chunkPath, FileMode.Create);
        await file.CopyToAsync(stream);

        info.ReceivedChunks.Add(chunkIndex);
        SaveMetadata(info);

        return ApiResult.Ok();
    }

    public ChunkStatusResult GetStatus(string uploadId)
    {
        if (!ChunkStore.TryGetValue(uploadId, out var info))
            return new ChunkStatusResult { UploadId = uploadId };

        return new ChunkStatusResult
        {
            UploadId = uploadId,
            ReceivedChunks = [.. info.ReceivedChunks.OrderBy(x => x)],
            IsCompleted = info.ReceivedChunks.Count >= info.TotalChunks
        };
    }

    public async Task<ApiResult<FileUploadResult>> MergeAsync(string uploadId, string? bizType = null, string? bizKey = null)
    {
        if (!ChunkStore.TryGetValue(uploadId, out var info))
            return ApiResult<FileUploadResult>.Fail(404, "上传会话不存在");

        if (info.ReceivedChunks.Count < info.TotalChunks)
            return ApiResult<FileUploadResult>.Fail(400, $"分片未完成: {info.ReceivedChunks.Count}/{info.TotalChunks}");

        var chunkDir = GetChunkDir(uploadId);
        var tempFile = Path.Combine(Path.GetTempPath(), $"merge_{uploadId}_{info.FileName}");

        try
        {
            await using var output = new FileStream(tempFile, FileMode.Create);
            for (var i = 0; i < info.TotalChunks; i++)
            {
                var chunkPath = Path.Combine(chunkDir, $"chunk_{i:D6}");
                if (!File.Exists(chunkPath))
                    return ApiResult<FileUploadResult>.Fail(400, $"分片 {i} 缺失");

                await using var input = File.OpenRead(chunkPath);
                await input.CopyToAsync(output);
            }

            await output.FlushAsync();
        }
        catch (Exception ex)
        {
            return ApiResult<FileUploadResult>.Fail(500, $"合并失败: {ex.Message}");
        }

        await using var fileStream = new FileStream(tempFile, FileMode.Open, FileAccess.Read);
        var formFile = new FormFile(fileStream, 0, fileStream.Length, "file", info.FileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/octet-stream"
        };

        var result = await _fileService.UploadAsync(formFile, bizType ?? info.BizType, bizKey ?? info.BizKey);

        Cleanup(uploadId);

        try { File.Delete(tempFile); } catch { }

        return result;
    }

    private string GetChunkDir(string uploadId) => Path.Combine(_tempDir, uploadId);

    private void SaveMetadata(ChunkInfo info)
    {
        try
        {
            var json = JsonSerializer.Serialize(info);
            File.WriteAllText(Path.Combine(GetChunkDir(info.UploadId), "metadata.json"), json);
        }
        catch { }
    }

    public void Cleanup(string uploadId)
    {
        ChunkStore.TryRemove(uploadId, out _);
        var dir = GetChunkDir(uploadId);
        if (Directory.Exists(dir))
        {
            try { Directory.Delete(dir, true); } catch { }
        }
    }
}
