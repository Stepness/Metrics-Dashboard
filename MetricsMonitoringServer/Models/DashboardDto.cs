namespace MetricsMonitoringServer.Models;

public class DashboardDto
{
    public string ConnectionId { get; set; }
    public long CPUUsagePercentage { get; set; }
    public string CPUName { get; set; }
    public long RamUsageMegabytes { get; set; }
    public int DiskFreePercentage { get; set; }
}