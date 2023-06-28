using MetricsMonitoringServer.Models;
using MetricsMonitoringServer.Settings;
using Microsoft.AspNetCore.SignalR;

namespace MetricsMonitoringServer;

public class MetricsHub : Hub
{
    public async Task SendBroadcastMessage(string connectionId, MetricsDto metrics)
    {
        await Clients.Group(HubSettings.DashboardGroup).SendAsync("ReceiveBroadcastMessage", connectionId, metrics);
    }

    public async Task JoinDashboardGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, HubSettings.DashboardGroup);
        Console.WriteLine("New dashboard joined");
    }
    
    public async Task JoinClientGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, HubSettings.ClientGroup);
        Console.WriteLine("New client joined");
    }
}