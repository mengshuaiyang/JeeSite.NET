using System.Diagnostics;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// 视频处理工具类（通过命令行调用外部 FFmpeg / FFprobe 实现截图、转码、元信息读取）
/// </summary>
public static class VideoUtil
{
    /// <summary>
    /// 在常见安装路径中查找 ffmpeg 可执行文件，通过运行 -version 验证可用性
    /// </summary>
    /// <returns>ffmpeg 可执行路径</returns>
    private static string FindFfmpeg()
    {
        foreach (var path in new[]
        {
            "ffmpeg",
            "ffmpeg.exe",
            Path.Combine(AppContext.BaseDirectory, "ffmpeg.exe"),
            Path.Combine(AppContext.BaseDirectory, "bin", "ffmpeg.exe"),
            @"C:\ffmpeg\bin\ffmpeg.exe",
            "/usr/bin/ffmpeg",
            "/usr/local/bin/ffmpeg"
        })
        {
            try
            {
                using var proc = Process.Start(new ProcessStartInfo(path, "-version")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
                if (proc != null) { proc.WaitForExit(2000); return path; }
            }
            catch { }
        }
        return null!;
    }

    /// <summary>
    /// 检查当前运行环境中是否安装了 FFmpeg
    /// </summary>
    /// <returns>可成功执行 ffmpeg -version 时返回 true；否则 false</returns>
    public static bool IsAvailable
    {
        get
        {
            try
            {
                var ffmpeg = FindFfmpeg();
                if (ffmpeg == null) return false;

                using var proc = Process.Start(new ProcessStartInfo(ffmpeg, "-version")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
                if (proc == null) return false;
                proc.WaitForExit(3000);
                return proc.ExitCode == 0;
            }
            catch { return false; }
        }
    }

    /// <summary>
    /// 在指定时间点截取视频缩略图（输出静态 JPEG/PNG 图片）
    /// </summary>
    /// <param name="inputVideo">输入视频文件路径</param>
    /// <param name="outputImage">输出图片路径</param>
    /// <param name="timeOffset">截图时间点（HH:mm:ss 或秒数）</param>
    /// <returns>成功且输出文件存在时返回 true；否则 false</returns>
    public static async Task<bool> ScreenshotAsync(string inputVideo, string outputImage, string timeOffset = "00:00:01")
    {
        var ffmpeg = FindFfmpeg();
        if (ffmpeg == null) throw new FileNotFoundException("ffmpeg 未找到, 请先安装 FFmpeg");

        // -ss 置于 -i 前以实现快速定位（关键帧）；-q:v 2 控制输出图像质量
        var psi = new ProcessStartInfo(ffmpeg)
        {
            Arguments = $"-y -ss {timeOffset} -i \"{inputVideo}\" -vframes 1 -q:v 2 \"{outputImage}\"",
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var proc = Process.Start(psi);
        if (proc == null) return false;
        await proc.WaitForExitAsync();
        return proc.ExitCode == 0 && File.Exists(outputImage);
    }

    /// <summary>
    /// 将任意格式视频转码为 MP4（H.264 视频 + AAC 音频）
    /// </summary>
    /// <param name="inputFile">输入视频文件路径</param>
    /// <param name="outputFile">输出 MP4 文件路径</param>
    /// <returns>成功且输出文件存在时返回 true；否则 false</returns>
    public static async Task<bool> ConvertToMp4Async(string inputFile, string outputFile)
    {
        var ffmpeg = FindFfmpeg();
        if (ffmpeg == null) throw new FileNotFoundException("ffmpeg 未找到, 请先安装 FFmpeg");

        // libx264 + -preset fast 平衡速度与压缩率；-crf 23 视觉无损；-b:a 128k 音频比特率
        var psi = new ProcessStartInfo(ffmpeg)
        {
            Arguments = $"-y -i \"{inputFile}\" -c:v libx264 -preset fast -crf 23 -c:a aac -b:a 128k \"{outputFile}\"",
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var proc = Process.Start(psi);
        if (proc == null) return false;
        await proc.WaitForExitAsync();
        return proc.ExitCode == 0 && File.Exists(outputFile);
    }

    /// <summary>
    /// 通过 ffprobe -of json 读取视频元信息（分辨率、编码、时长、大小、比特率）
    /// </summary>
    /// <param name="videoFile">视频文件路径</param>
    /// <returns>VideoInfo 对象；文件不存在或 ffprobe 执行失败时返回 null</returns>
    public static async Task<VideoInfo?> GetInfoAsync(string videoFile)
    {
        if (!File.Exists(videoFile)) return null;

        var ffprobe = FindFfprobe();
        if (ffprobe == null) return null;

        // 以 JSON 格式输出 format（容器）与 streams（音视频流）关键字段
        var psi = new ProcessStartInfo(ffprobe)
        {
            Arguments = $"-v error -show_entries format=duration,size,bit_rate:stream=width,height,codec_name,codec_type -of json \"{videoFile}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var proc = Process.Start(psi);
        if (proc == null) return null;

        var output = await proc.StandardOutput.ReadToEndAsync();
        await proc.WaitForExitAsync();
        if (proc.ExitCode != 0) return null;

        try
        {
            var doc = System.Text.Json.JsonDocument.Parse(output);
            var info = new VideoInfo();

            // 读取 format 段：时长 / 文件大小 / 比特率
            var format = doc.RootElement.GetProperty("format");
            if (format.TryGetProperty("duration", out var dur))
                info.DurationSeconds = double.Parse(dur.GetString() ?? "0");
            if (format.TryGetProperty("size", out var size))
                info.FileSizeBytes = long.Parse(size.GetString() ?? "0");
            if (format.TryGetProperty("bit_rate", out var bitrate))
                info.BitRate = int.Parse(bitrate.GetString() ?? "0");

            // 读取 streams 段：按 codec_type 区分 video/audio
            var streams = doc.RootElement.GetProperty("streams");
            foreach (var stream in streams.EnumerateArray())
            {
                var codecType = stream.GetProperty("codec_type").GetString();
                if (codecType == "video")
                {
                    info.Width = stream.GetProperty("width").GetInt32();
                    info.Height = stream.GetProperty("height").GetInt32();
                    info.VideoCodec = stream.GetProperty("codec_name").GetString();
                }
                else if (codecType == "audio")
                {
                    info.AudioCodec = stream.GetProperty("codec_name").GetString();
                }
            }

            return info;
        }
        catch { return null; }
    }

    /// <summary>
    /// 在常见安装路径中查找 ffprobe 可执行文件，通过运行 -version 验证可用性
    /// </summary>
    /// <returns>ffprobe 可执行路径</returns>
    private static string FindFfprobe()
    {
        foreach (var path in new[]
        {
            "ffprobe",
            "ffprobe.exe",
            Path.Combine(AppContext.BaseDirectory, "ffprobe.exe"),
            Path.Combine(AppContext.BaseDirectory, "bin", "ffprobe.exe"),
            @"C:\ffmpeg\bin\ffprobe.exe",
            "/usr/bin/ffprobe",
            "/usr/local/bin/ffprobe"
        })
        {
            try
            {
                using var proc = Process.Start(new ProcessStartInfo(path, "-version")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
                if (proc != null) { proc.WaitForExit(2000); return path; }
            }
            catch { }
        }
        return null!;
    }
}

/// <summary>
/// 视频元信息容器（分辨率、编码、时长、大小、比特率、格式化输出）
/// </summary>
public class VideoInfo
{
    /// <summary>
    /// 视频宽度（像素）
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// 视频高度（像素）
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// 视频编码名称（如 h264 / hevc / vp9）
    /// </summary>
    public string? VideoCodec { get; set; }

    /// <summary>
    /// 音频编码名称（如 aac / mp3 / opus）
    /// </summary>
    public string? AudioCodec { get; set; }

    /// <summary>
    /// 总时长（秒）
    /// </summary>
    public double DurationSeconds { get; set; }

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    public long FileSizeBytes { get; set; }

    /// <summary>
    /// 比特率（bps）
    /// </summary>
    public int BitRate { get; set; }

    /// <summary>
    /// 时长格式化字符串（hh:mm:ss）
    /// </summary>
    public string DurationFormatted => TimeSpan.FromSeconds(DurationSeconds).ToString(@"hh\:mm\:ss");

    /// <summary>
    /// 文件大小格式化字符串（B / KB / MB）
    /// </summary>
    public string FileSizeFormatted => FileSizeBytes switch
    {
        < 1024 => $"{FileSizeBytes} B",
        < 1024 * 1024 => $"{FileSizeBytes / 1024.0:F1} KB",
        _ => $"{FileSizeBytes / (1024.0 * 1024.0):F1} MB"
    };

    /// <summary>
    /// 分辨率组合字符串（如 "1920x1080"）
    /// </summary>
    public string Resolution => $"{Width}x{Height}";
}
