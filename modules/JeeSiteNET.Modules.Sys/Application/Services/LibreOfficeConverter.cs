using System.Diagnostics;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public static class LibreOfficeConverter
{
    private static readonly string[] CandidatePaths =
    [
        @"C:\Program Files\LibreOffice\program\soffice.exe",
        @"C:\Program Files (x86)\LibreOffice\program\soffice.exe",
        "/usr/lib/libreoffice/program/soffice",
        "/usr/bin/soffice",
    ];

    public static async Task<string> ConvertToPdfAsync(string inputPath)
    {
        var soffice = FindSoffice();
        if (soffice == null)
            throw new InvalidOperationException("LibreOffice 未安装。请安装 LibreOffice 并确保 soffice 在 PATH 中或安装在默认位置。");

        var outputDir = Path.Combine(Path.GetTempPath(), "jeesite_preview", "pdf");
        Directory.CreateDirectory(outputDir);

        var psi = new ProcessStartInfo
        {
            FileName = soffice,
            Arguments = $"--headless --convert-to pdf --outdir \"{outputDir}\" \"{inputPath}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using var process = Process.Start(psi);
        if (process == null)
            throw new InvalidOperationException("无法启动 LibreOffice 进程");

        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            var err = await process.StandardError.ReadToEndAsync();
            throw new InvalidOperationException($"LibreOffice 转换失败 (exit: {process.ExitCode}): {err}");
        }

        var pdfName = Path.GetFileNameWithoutExtension(inputPath) + ".pdf";
        return Path.Combine(outputDir, pdfName);
    }

    private static string? FindSoffice()
    {
        foreach (var path in CandidatePaths)
            if (File.Exists(path))
                return path;

        try
        {
            var psi = new ProcessStartInfo("soffice", "--version")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            using var proc = Process.Start(psi);
            if (proc != null) return "soffice";
        }
        catch { }

        return null;
    }
}
