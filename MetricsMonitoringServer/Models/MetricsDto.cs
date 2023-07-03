namespace MetricsMonitoringServer.Models;

public class MetricsDto
{
    public long CpuUsagePercentage { get; set; }
    public string CpuName { get; set; } = string.Empty;
    public long RamUsageMegabytes { get; set; }
    public int DiskFreePercentage { get; set; }
    public long Eth0TransmittedBytes { get; set; }
}