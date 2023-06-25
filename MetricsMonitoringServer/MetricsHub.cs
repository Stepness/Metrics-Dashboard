using MetricsMonitoringServer.Models;
using Microsoft.AspNetCore.SignalR;

namespace MetricsMonitoringServer;

public class MetricsHub : Hub
{
    public async Task ReceiveMessage(string connectionId, MetricsDto metrics)
    {
    }
}