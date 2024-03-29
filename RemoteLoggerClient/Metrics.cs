using System.Diagnostics;
using System.Net.NetworkInformation;

namespace RemoteLoggerClient;

public static class Metrics
{
    private static string _cpuName = string.Empty;

    public static async Task<string> GetCpuNameAsync()
    {
        if (string.IsNullOrEmpty(_cpuName))
        {
            _cpuName = await RetrieveCpuAsync();
        }

        return _cpuName;
    }

    public static long GetRamUsageMegabytes()
    {
        var processes = Process.GetProcesses();
        return processes.Sum(x => x.PrivateMemorySize64) / 1024 / 1024;
    }

    public static int GetDiskSpacePercentage()
    {
        var drive = new DriveInfo("."); //https://stackoverflow.com/a/62973033

        var totalGigaBytes = drive.TotalSize / 1024 / 1024 / 1024;
        var freeGigaBytes = drive.AvailableFreeSpace / 1024 / 1024 / 1024;

        var freePercent = (int)(100 * freeGigaBytes / totalGigaBytes);

        return freePercent;
    }

    private static async Task<string> RetrieveCpuAsync()
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
    public static async Task<long> GetCpuUsageAsync()
    {
        using var reader = new StreamReader("/proc/stat");
        var line = await reader.ReadLineAsync();

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

    public static long GetTransmittedBytesForEth0()
    {
        var interfaces = NetworkInterface.GetAllNetworkInterfaces();

        var eth0If = interfaces
            .FirstOrDefault(x => x is { Name: "eth0", OperationalStatus: OperationalStatus.Up });

        return eth0If?.GetIPv4Statistics().BytesSent ?? 0; 

    }
}