    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Utils 命名空间
// 引入命名空间：JeeSiteNET.Core.Utils
using JeeSiteNET.Core.Utils;
    // 引入 System.Collections.Concurrent 命名空间
// 引入命名空间：System.Collections.Concurrent
using System.Collections.Concurrent;
    // 引入 System.Text.Json 命名空间
// 引入命名空间：System.Text.Json
using System.Text.Json;
    // 引入 Microsoft.AspNetCore.Http 命名空间
// 引入命名空间：Microsoft.AspNetCore.Http
using Microsoft.AspNetCore.Http;

// 定义 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Application.Services
namespace JeeSiteNET.Modules.Sys.Application.Services;

// 定义class ChunkInfo
// 定义类：ChunkInfo
public class ChunkInfo
{
    // 属性 UploadId
    // 属性：UploadId
    public string UploadId { get; set; } = string.Empty;
    // 属性 FileName
    // 属性：FileName
    public string FileName { get; set; } = string.Empty;
    // 属性 TotalSize
    // 属性：TotalSize
    public long TotalSize { get; set; }
    // 属性 ChunkSize
    // 属性：ChunkSize
    public long ChunkSize { get; set; }
    // 属性 TotalChunks
    // 属性：TotalChunks
    public int TotalChunks { get; set; }
    // 属性：FileMd5
    public string? FileMd5 { get; set; }
    // 属性：BizType
    public string? BizType { get; set; }
    // 属性：BizKey
    public string? BizKey { get; set; }
    // 属性 ReceivedChunks
    // 属性：ReceivedChunks
    public HashSet<int> ReceivedChunks { get; set; } = [];
    // 属性 CreateTime
    // 属性：CreateTime
    public DateTime CreateTime { get; set; } = DateTime.Now;
}

// 定义class ChunkInitRequest
// 定义类：ChunkInitRequest
public class ChunkInitRequest
{
    // 属性 FileName
    // 属性：FileName
    public string FileName { get; set; } = string.Empty;
    // 属性 TotalSize
    // 属性：TotalSize
    public long TotalSize { get; set; }
    // 属性 ChunkSize
    // 属性：ChunkSize
    public long ChunkSize { get; set; }
    // 属性 TotalChunks
    // 属性：TotalChunks
    public int TotalChunks { get; set; }
    // 属性：FileMd5
    public string? FileMd5 { get; set; }
    // 属性：BizType
    public string? BizType { get; set; }
    // 属性：BizKey
    public string? BizKey { get; set; }
}

// 定义class ChunkInitResult
// 定义类：ChunkInitResult
public class ChunkInitResult
{
    // 属性 UploadId
    // 属性：UploadId
    public string UploadId { get; set; } = string.Empty;
    // 属性 Exist
    // 属性：Exist
    public bool Exist { get; set; }
    // 属性：FileUrl
    public string? FileUrl { get; set; }
    // 属性：UploadId2
    public string? UploadId2 { get; set; }
}

// 定义class ChunkStatusResult
// 定义类：ChunkStatusResult
public class ChunkStatusResult
{
    // 属性 UploadId
    // 属性：UploadId
    public string UploadId { get; set; } = string.Empty;
    // 属性 ReceivedChunks
    // 属性：ReceivedChunks
    public List<int> ReceivedChunks { get; set; } = [];
    // 属性 IsCompleted
    // 属性：IsCompleted
    public bool IsCompleted { get; set; }
}

// 定义class ChunkUploadService
// 定义类：ChunkUploadService
public class ChunkUploadService
{
    private static readonly ConcurrentDictionary<string, ChunkInfo> ChunkStore = new();
    // 字段 _fileService
    // 字段：_fileService
    private readonly FileService _fileService;
    // 字段 _tempDir
    // 字段：_tempDir
    private readonly string _tempDir;

    // 方法 ChunkUploadService
    // 构造函数：ChunkUploadService
    public ChunkUploadService(FileService fileService)
    {
        _fileService = fileService;
        _tempDir = Path.Combine(Path.GetTempPath(), "jeesite-chunks");
        Directory.CreateDirectory(_tempDir);
    }

    // 方法 InitAsync
    // 方法：InitAsync
    public async Task<ChunkInitResult> InitAsync(ChunkInitRequest request)
    {
        // if 条件判断
        if (!string.IsNullOrEmpty(request.FileMd5))
        {
            var existResult = await _fileService.CheckMd5ExistAsync(request.FileMd5);
            // if 条件判断
            if (existResult != null)
            {
                // return 返回结果
                return new ChunkInitResult { UploadId = existResult.UploadId, Exist = true, FileUrl = existResult.FileUrl, UploadId2 = existResult.UploadId2 };
            }
        }

        // 声明并初始化变量：uploadId
        var uploadId = IdGenerator.NewId();
        // 创建 ChunkInfo实例并赋给 info
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

        // 声明并初始化变量：dir
        var dir = GetChunkDir(uploadId);
        Directory.CreateDirectory(dir);

        ChunkStore[uploadId] = info;
        SaveMetadata(info);

        // return 返回结果
        return new ChunkInitResult { UploadId = uploadId, Exist = false };
    }

    // 方法 UploadChunkAsync
    // 方法：UploadChunkAsync
    public async Task<ApiResult> UploadChunkAsync(string uploadId, int chunkIndex, IFormFile file)
    {
        // if 条件判断
        if (!ChunkStore.TryGetValue(uploadId, out var info))
            // return 返回结果
            return ApiResult.Fail(404, "上传会话不存在");

        // 声明并初始化变量：chunkDir
        var chunkDir = GetChunkDir(uploadId);
        // 声明并初始化变量：chunkPath
        var chunkPath = Path.Combine(chunkDir, $"chunk_{chunkIndex:D6}");

        // await 异步等待
        await using var stream = new FileStream(chunkPath, FileMode.Create);
        // await 异步等待
        await file.CopyToAsync(stream);

        // 集合操作：添加元素
        info.ReceivedChunks.Add(chunkIndex);
        SaveMetadata(info);

        // return 返回结果
        return ApiResult.Ok();
    }

    // 方法 GetStatus
    // 方法：GetStatus
    public ChunkStatusResult GetStatus(string uploadId)
    {
        // if 条件判断
        if (!ChunkStore.TryGetValue(uploadId, out var info))
            // return 返回结果
            return new ChunkStatusResult { UploadId = uploadId };

        // return 返回结果
        return new ChunkStatusResult
        {
            UploadId = uploadId,
            // 数据库操作：升序排序
            ReceivedChunks = [.. info.ReceivedChunks.OrderBy(x => x)],
            IsCompleted = info.ReceivedChunks.Count >= info.TotalChunks
        };
    }

    // 方法 MergeAsync
    // 方法：MergeAsync
    public async Task<ApiResult<FileUploadResult>> MergeAsync(string uploadId, string? bizType = null, string? bizKey = null)
    {
        // if 条件判断
        if (!ChunkStore.TryGetValue(uploadId, out var info))
            // return 返回结果
            return ApiResult<FileUploadResult>.Fail(404, "上传会话不存在");

        // if 条件判断
        if (info.ReceivedChunks.Count < info.TotalChunks)
            // return 返回结果
            return ApiResult<FileUploadResult>.Fail(400, $"分片未完成: {info.ReceivedChunks.Count}/{info.TotalChunks}");

        // 声明并初始化变量：chunkDir
        var chunkDir = GetChunkDir(uploadId);
        // 声明并初始化变量：tempFile
        var tempFile = Path.Combine(Path.GetTempPath(), $"merge_{uploadId}_{info.FileName}");

        // try 异常捕获开始
        try
        {
            // await 异步等待
            await using var output = new FileStream(tempFile, FileMode.Create);
            // for 循环
            for (var i = 0; i < info.TotalChunks; i++)
            {
                // 声明并初始化变量：chunkPath
                var chunkPath = Path.Combine(chunkDir, $"chunk_{i:D6}");
                // if 条件判断
                if (!File.Exists(chunkPath))
                    // return 返回结果
                    return ApiResult<FileUploadResult>.Fail(400, $"分片 {i} 缺失");

                // await 异步等待
                await using var input = File.OpenRead(chunkPath);
                // await 异步等待
                await input.CopyToAsync(output);
            }

            // await 异步等待
            await output.FlushAsync();
        }
        // catch 捕获异常
        catch (Exception ex)
        {
            // return 返回结果
            return ApiResult<FileUploadResult>.Fail(500, $"合并失败: {ex.Message}");
        }

        // await 异步等待
        await using var fileStream = new FileStream(tempFile, FileMode.Open, FileAccess.Read);
        // 创建 FormFile实例并赋给 formFile
        var formFile = new FormFile(fileStream, 0, fileStream.Length, "file", info.FileName)
        {
            // 创建 HeaderDictionary实例并赋给 Headers
            Headers = new HeaderDictionary(),
            ContentType = "application/octet-stream"
        };

        // null 合并操作 ??（若为 null 则使用右侧值）
        var result = await _fileService.UploadAsync(formFile, bizType ?? info.BizType, bizKey ?? info.BizKey);

        Cleanup(uploadId);

        // 数据库操作：删除
        try { File.Delete(tempFile); } catch { }

        // return 返回结果
        return result;
    }

    // 方法：GetChunkDir
    private string GetChunkDir(string uploadId) => Path.Combine(_tempDir, uploadId);

    // 方法 SaveMetadata
    // 方法：SaveMetadata
    private void SaveMetadata(ChunkInfo info)
    {
        // try 异常捕获开始
        try
        {
            // 声明并初始化变量：json
            var json = JsonSerializer.Serialize(info);
            // 文件/流操作：写入文本到文件
            File.WriteAllText(Path.Combine(GetChunkDir(info.UploadId), "metadata.json"), json);
        }
        catch { }
    }

    // 方法 Cleanup
    // 方法：Cleanup
    public void Cleanup(string uploadId)
    {
        // 集合操作：尝试移除
        ChunkStore.TryRemove(uploadId, out _);
        // 声明并初始化变量：dir
        var dir = GetChunkDir(uploadId);
        // if 条件判断
        if (Directory.Exists(dir))
        {
            // 数据库操作：删除
            try { Directory.Delete(dir, true); } catch { }
        }
    }
}
