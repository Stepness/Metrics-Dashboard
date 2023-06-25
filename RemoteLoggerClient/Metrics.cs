using System.Diagnostics;

namespace RemoteLoggerClient;

public static class Metrics
{
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
    
    
    public static async Task<string> CpuNameAsync()
    {
        var cpuName = string.Empty;

        while (!File.Exists("/proc/cpuinfo"))
        {
            
        }
        using var reader = new StreamReader("/proc/cpuinfo");
        string line;

        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (line.StartsWith("model name"))
            {
                var startIndex = line.IndexOf(':') + 2;
                cpuName = line[startIndex..];
                break;
            }
        }

        return cpuName;
    }
    
    // https://www.baeldung.com/linux/get-cpu-usage#2-getting-cpu-usage-using-procstat
    public static long CpuUsage()
    {
        using var reader = new StreamReader("/proc/stat");
        var line = reader.ReadLine();

        if (line != null && line.StartsWith("cpu "))
        {
            var cpuStats = line
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .ToArray();

            var idle = long.Parse(cpuStats[3]);
            var total = cpuStats.Sum(x => long.Parse(x));

            var averageIdleTime = (idle * 100) / total;
            return 100 - averageIdleTime;
        }

        return 0;
    }
    
    public static void NetworkInfo()
    {
        using (var reader = new StreamReader("/proc/net/dev"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains(":"))
                {
                    string[] parts = line.Split(':', StringSplitOptions.RemoveEmptyEntries);
                    string interfaceName = parts[0].Trim();
                    string[] stats = parts[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    long receivedBytes = long.Parse(stats[0]);
                    long transmittedBytes = long.Parse(stats[8]);

                    Console.WriteLine($"Interface: {interfaceName}");
                    Console.WriteLine($"Received Bytes: {receivedBytes}");
                    Console.WriteLine($"Transmitted Bytes: {transmittedBytes}");
                    Console.WriteLine();
                }
            }
        }
    }

}

