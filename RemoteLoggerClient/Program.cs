using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Text.RegularExpressions;


while (true)
{
    Console.WriteLine(GetRamUsage());
    Console.WriteLine(GetDiskSpace());
    Console.WriteLine(GetCpuName());
    Console.WriteLine("Cpu: " + GetCpuUsage());
    GetNetworkInfo();
    await Task.Delay(3000);
}

static long GetRamUsage()
{
    {
        var proc = Process.GetProcesses();
        return proc.Sum(x => x.PrivateMemorySize64) / 1024 / 1024;
    }
}

static int GetDiskSpace()
{
    var drive = new DriveInfo("."); //https://stackoverflow.com/a/62973033

    var totalGigaBytes = drive.TotalSize / 1024 / 1024 / 1024;
    var freeGigaBytes = drive.AvailableFreeSpace / 1024 / 1024 / 1024;

    var freePercent = (int)((100 * freeGigaBytes) / totalGigaBytes);

    return freePercent;
}

static string GetCpuName()
{
    string cpuName = string.Empty;
    using var reader = new StreamReader("/proc/cpuinfo");
    string line;

    while ((line = reader.ReadLine()) != null)
    {
        if (line.StartsWith("model name"))
        {
            int startIndex = line.IndexOf(':') + 2;
            cpuName = line[startIndex..];
            break;
        }
    }

    return cpuName;
}

// https://www.baeldung.com/linux/get-cpu-usage#2-getting-cpu-usage-using-procstat
static long GetCpuUsage()
{
    using var reader = new StreamReader("/proc/stat");
    string line = reader.ReadLine();

    if (line != null && line.StartsWith("cpu "))
    {
        var cpuStats = line
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .ToArray();

        var idle = long.Parse(cpuStats[3]);
        var total = cpuStats.Sum(x=>long.Parse(x));

        var averageIdleTime = (idle * 100) / total;
        return 100 - averageIdleTime;
    }

    return 0;
}

static void GetNetworkInfo()
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