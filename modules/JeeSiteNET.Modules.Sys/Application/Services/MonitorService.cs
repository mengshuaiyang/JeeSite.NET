using System.Diagnostics;
using System.Runtime.InteropServices;

namespace JeeSiteNET.Modules.Sys.Application.Services;

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

public class MonitorService
{
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

    private static double GetCpuUsage(Process process)
    {
        try
        {
            var startTime = DateTime.UtcNow;
            var startCpu = process.TotalProcessorTime;
            Thread.Sleep(500);
            var endTime = DateTime.UtcNow;
            var endCpu = process.TotalProcessorTime;
            var cpuUsedMs = (endCpu - startCpu).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            return Math.Round(cpuUsedMs / (totalMsPassed * Environment.ProcessorCount) * 100, 2);
        }
        catch
        {
            return 0;
        }
    }

    private static List<DiskInfo> GetDiskInfo()
    {
        var disks = new List<DiskInfo>();
        try
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
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
