    // 引入 System.Diagnostics 命名空间
// 引入命名空间：System.Diagnostics
using System.Diagnostics;

// 定义 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Application.Services
namespace JeeSiteNET.Modules.Sys.Application.Services;

// 定义class LibreOfficeConverter
// 定义类：LibreOfficeConverter
public static class LibreOfficeConverter
{
    private static readonly string[] CandidatePaths =
    [
        @"C:\Program Files\LibreOffice\program\soffice.exe",
        @"C:\Program Files (x86)\LibreOffice\program\soffice.exe",
        "/usr/lib/libreoffice/program/soffice",
        "/usr/bin/soffice",
    ];

    // 方法 ConvertToPdfAsync
    // 方法：ConvertToPdfAsync
    public static async Task<string> ConvertToPdfAsync(string inputPath)
    {
        // 声明并初始化变量：soffice
        var soffice = FindSoffice();
        // if 条件判断
        if (soffice == null)
            // throw 抛出异常
            throw new InvalidOperationException("LibreOffice 未安装。请安装 LibreOffice 并确保 soffice 在 PATH 中或安装在默认位置。");

        // 声明并初始化变量：outputDir
        var outputDir = Path.Combine(Path.GetTempPath(), "jeesite_preview", "pdf");
        Directory.CreateDirectory(outputDir);

        // 创建 ProcessStartInfo实例并赋给 psi
        var psi = new ProcessStartInfo
        {
            FileName = soffice,
            Arguments = $"--headless --convert-to pdf --outdir \"{outputDir}\" \"{inputPath}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

    // 引入 var process 命名空间
        // 调用 Start
        using var process = Process.Start(psi);
        // if 条件判断
        if (process == null)
            // throw 抛出异常
            throw new InvalidOperationException("无法启动 LibreOffice 进程");

        // await 异步等待
        await process.WaitForExitAsync();

        // if 条件判断
        if (process.ExitCode != 0)
        {
            var err = await process.StandardError.ReadToEndAsync();
            // throw 抛出异常
            throw new InvalidOperationException($"LibreOffice 转换失败 (exit: {process.ExitCode}): {err}");
        }

        // 声明并初始化变量：pdfName
        var pdfName = Path.GetFileNameWithoutExtension(inputPath) + ".pdf";
        // return 返回结果
        return Path.Combine(outputDir, pdfName);
    }

    // 方法 FindSoffice
    // 方法：FindSoffice
    private static string? FindSoffice()
    {
        // foreach 遍历集合
        foreach (var path in CandidatePaths)
            // if 条件判断
            if (File.Exists(path))
                // return 返回结果
                return path;

        // try 异常捕获开始
        try
        {
            // 创建 ProcessStartInfo实例并赋给 psi
            var psi = new ProcessStartInfo("soffice", "--version")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
    // 引入 var proc 命名空间
            // 调用 Start
            using var proc = Process.Start(psi);
            // if 条件判断
            if (proc != null) return "soffice";
        }
        catch { }

        // return 返回结果
        return null;
    }
}
