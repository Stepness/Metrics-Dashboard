namespace RemoteLoggerClient.Models;

public class MetricsDto
{
    public long CPUUsagePercentage { get; set; }
    public string CPUName { get; set; }
    public long RamUsageMegabytes { get; set; }
    public int DiskFreePercentage { get; set; }
}