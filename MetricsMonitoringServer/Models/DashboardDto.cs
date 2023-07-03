namespace MetricsMonitoringServer.Models;

public class DashboardDto
{
    public string ConnectionId { get; set; } = string.Empty;
    public long CpuUsagePercentage { get; set; }
    public string CpuName { get; set; } = string.Empty;
    public long RamUsageMegabytes { get; set; }
    public int DiskFreePercentage { get; set; }
    public long Eth0TransmittedBytes { get; set; }
}