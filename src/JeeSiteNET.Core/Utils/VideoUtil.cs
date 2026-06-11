using System.Diagnostics;

namespace JeeSiteNET.Core.Utils;

public static class VideoUtil
{
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

    public static async Task<bool> ScreenshotAsync(string inputVideo, string outputImage, string timeOffset = "00:00:01")
    {
        var ffmpeg = FindFfmpeg();
        if (ffmpeg == null) throw new FileNotFoundException("ffmpeg 未找到, 请先安装 FFmpeg");

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

    public static async Task<bool> ConvertToMp4Async(string inputFile, string outputFile)
    {
        var ffmpeg = FindFfmpeg();
        if (ffmpeg == null) throw new FileNotFoundException("ffmpeg 未找到, 请先安装 FFmpeg");

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

    public static async Task<VideoInfo?> GetInfoAsync(string videoFile)
    {
        if (!File.Exists(videoFile)) return null;

        var ffprobe = FindFfprobe();
        if (ffprobe == null) return null;

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

            var format = doc.RootElement.GetProperty("format");
            if (format.TryGetProperty("duration", out var dur))
                info.DurationSeconds = double.Parse(dur.GetString() ?? "0");
            if (format.TryGetProperty("size", out var size))
                info.FileSizeBytes = long.Parse(size.GetString() ?? "0");
            if (format.TryGetProperty("bit_rate", out var bitrate))
                info.BitRate = int.Parse(bitrate.GetString() ?? "0");

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

public class VideoInfo
{
    public int Width { get; set; }
    public int Height { get; set; }
    public string? VideoCodec { get; set; }
    public string? AudioCodec { get; set; }
    public double DurationSeconds { get; set; }
    public long FileSizeBytes { get; set; }
    public int BitRate { get; set; }
    public string DurationFormatted => TimeSpan.FromSeconds(DurationSeconds).ToString(@"hh\:mm\:ss");
    public string FileSizeFormatted => FileSizeBytes switch
    {
        < 1024 => $"{FileSizeBytes} B",
        < 1024 * 1024 => $"{FileSizeBytes / 1024.0:F1} KB",
        _ => $"{FileSizeBytes / (1024.0 * 1024.0):F1} MB"
    };
    public string Resolution => $"{Width}x{Height}";
}
