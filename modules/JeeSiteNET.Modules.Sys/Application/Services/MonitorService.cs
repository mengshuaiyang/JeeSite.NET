using System.Diagnostics;
using System.Runtime.InteropServices;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>服务器信息 DTO。</summary>
public class ServerInfo
{
    public string OsName { get; set; } = string.Empty;
    public string OsVersion { get; set; } = string.Empty;
    public string OsArchitecture { get; set; } = string.Empty;
    public string ProcessArchitecture { get; set; } = string.Empty;
    public string MachineName { get; set; } = string.Empty;
    public string RuntimeVersion { get; set; } = string.Empty;
    public int ProcessorCount { get; set; }
    public DateTime StartTime { get; set; }
    public double UptimeDays { get; set; }
    public long ProcessMemoryWorkingSet { get; set; }
    public long ProcessMemoryPrivateBytes { get; set; }
    public long ProcessMemoryVirtualBytes { get; set; }
    public long GcTotalMemory { get; set; }
    public int ThreadCount { get; set; }
    public int HandleCount { get; set; }
    public List<DiskInfo> Disks { get; set; } = [];
    public double CpuUsagePercent { get; set; }
}

/// <summary>磁盘分区信息 DTO。</summary>
public class DiskInfo
{
    public string Name { get; set; } = string.Empty;
    public string DriveType { get; set; } = string.Empty;
    public string DriveFormat { get; set; } = string.Empty;
    public long TotalSize { get; set; }
    public long AvailableFreeSpace { get; set; }
    public long UsedSpace { get; set; }
    public double UsagePercent { get; set; }
}

/// <summary>服务器监控服务，负责读取当前进程/主机的 CPU、内存、磁盘等运行指标。</summary>
public class MonitorService
{
    /// <summary>获取综合服务器信息（OS、运行时、CPU、内存、磁盘分区）。</summary>
    /// <returns>服务器信息对象。</returns>
    public ServerInfo GetServerInfo()
    {
        var process = Process.GetCurrentProcess();
        var startTime = process.StartTime;

        var info = new ServerInfo
        {
            OsName = RuntimeInformation.OSDescription,
            OsVersion = Environment.OSVersion.VersionString,
            OsArchitecture = RuntimeInformation.OSArchitecture.ToString(),
            ProcessArchitecture = RuntimeInformation.ProcessArchitecture.ToString(),
            MachineName = Environment.MachineName,
            RuntimeVersion = RuntimeInformation.FrameworkDescription,
            ProcessorCount = Environment.ProcessorCount,
            StartTime = startTime,
            // 运行天数保留两位小数
            UptimeDays = Math.Round((DateTime.Now - startTime).TotalDays, 2),
            ProcessMemoryWorkingSet = process.WorkingSet64,
            ProcessMemoryPrivateBytes = process.PrivateMemorySize64,
            ProcessMemoryVirtualBytes = process.VirtualMemorySize64,
            GcTotalMemory = GC.GetTotalMemory(false),
            ThreadCount = process.Threads.Count,
            HandleCount = process.HandleCount,
            CpuUsagePercent = GetCpuUsage(process),
            Disks = GetDiskInfo()
        };

        return info;
    }

    /// <summary>采样当前进程的 CPU 使用率（两次读取 TotalProcessorTime 求差值，耗时约 500ms）。</summary>
    /// <param name="process">当前进程。</param>
    /// <returns>CPU 使用率百分比（0-100）。</returns>
    private static double GetCpuUsage(Process process)
    {
        try
        {
            var startTime = DateTime.UtcNow;
            var startCpu = process.TotalProcessorTime;
            // 阻塞 500ms 采样，这是得到稳定 CPU 使用率的必要窗口
            Thread.Sleep(500);
            var endTime = DateTime.UtcNow;
            var endCpu = process.TotalProcessorTime;
            var cpuUsedMs = (endCpu - startCpu).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            // 除以 ProcessorCount 以获得单核视角的使用率（0-100）
            return Math.Round(cpuUsedMs / (totalMsPassed * Environment.ProcessorCount) * 100, 2);
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>读取本地磁盘分区列表（跳过未就绪的光驱/网络盘）。</summary>
    /// <returns>磁盘分区列表。</returns>
    private static List<DiskInfo> GetDiskInfo()
    {
        var disks = new List<DiskInfo>();
        try
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                // IsReady 为 false 的设备（如空光驱）读取属性会抛异常，直接跳过
                if (!drive.IsReady) continue;
                disks.Add(new DiskInfo
                {
                    Name = drive.Name,
                    DriveType = drive.DriveType.ToString(),
                    DriveFormat = drive.DriveFormat,
                    TotalSize = drive.TotalSize,
                    AvailableFreeSpace = drive.AvailableFreeSpace,
                    UsedSpace = drive.TotalSize - drive.AvailableFreeSpace,
                    UsagePercent = drive.TotalSize > 0
                        ? Math.Round((double)(drive.TotalSize - drive.AvailableFreeSpace) / drive.TotalSize * 100, 2)
                        : 0
                });
            }
        }
        catch { }
        return disks;
    }
}
