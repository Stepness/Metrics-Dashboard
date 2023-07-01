using System.Diagnostics;

namespace RemoteLoggerClient;

public static class Metrics
{
    private static string cpuName;

    public static async Task<string> CpuNameAsync()
    {
        if (string.IsNullOrEmpty(cpuName))
        {
            cpuName = await GetCpuNameAsync();
        }

        return cpuName;
    }
    
    public static long RamUsageMegabytes()
    {
        var processes = Process.GetProcesses();
        return processes.Sum(x => x.PrivateMemorySize64) / 1024 / 1024;
    }

    public static int DiskSpace()
    {
        var drive = new DriveInfo("."); //https://stackoverflow.com/a/62973033

        var totalGigaBytes = drive.TotalSize / 1024 / 1024 / 1024;
        var freeGigaBytes = drive.AvailableFreeSpace / 1024 / 1024 / 1024;

        var freePercent = (int)(100 * freeGigaBytes / totalGigaBytes);

        return freePercent;
    }
    
    
    private static async Task<string> GetCpuNameAsync()
    {
        var cpu = Environment.GetEnvironmentVariable("CPU_NAME");

        if (!string.IsNullOrEmpty(cpu))
        {
            return cpu;
        }
        
        using var reader = new StreamReader("/proc/cpuinfo");

        while (await reader.ReadLineAsync() is { } line)
        {
            if (!line.StartsWith("model name")) continue;
            var startIndex = line.IndexOf(':') + 2;
            cpu = line[startIndex..];
            break;
        }

        return cpu!;
    }
    
    // https://www.baeldung.com/linux/get-cpu-usage#2-getting-cpu-usage-using-procstat
    public static long CpuUsage()
    {
        using var reader = new StreamReader("/proc/stat");
        var line = reader.ReadLine();

        if (line == null || !line.StartsWith("cpu ")) return 0;
        var cpuStats = line
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .ToArray();

        var idle = long.Parse(cpuStats[3]);
        var total = cpuStats.Sum(long.Parse);

        var averageIdleTime = (idle * 100) / total;
        return 100 - averageIdleTime;

    }
    
    public static async Task NetworkInfo()
    {
        using var reader = new StreamReader("/proc/net/dev");
        while (await reader.ReadLineAsync() is { } line)
        {
            if (!line.Contains(':')) continue;
            var parts = line.Split(':', StringSplitOptions.RemoveEmptyEntries);
            var interfaceName = parts[0].Trim();
            var stats = parts[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var receivedBytes = long.Parse(stats[0]);
            var transmittedBytes = long.Parse(stats[8]);

            Console.WriteLine($"Interface: {interfaceName}");
            Console.WriteLine($"Received Bytes: {receivedBytes}");
            Console.WriteLine($"Transmitted Bytes: {transmittedBytes}");
            Console.WriteLine();
        }
    }

}

