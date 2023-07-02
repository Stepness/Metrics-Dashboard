using AutoMapper;
using MetricsMonitoringServer.Models;
using MetricsMonitoringServer.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MetricsMonitoringServer;

public class MetricsHub : Hub
{
    private readonly IMapper _mapper;

    public MetricsHub(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public async Task SendBroadcastMessage(string connectionId, MetricsDto metrics)
    {
        var dashboardDto = _mapper.Map<DashboardDto>(metrics);
        dashboardDto.ConnectionId = connectionId;
        
        await Clients.Group(HubSettings.DashboardGroup).SendAsync("ReceiveBroadcastMessage", dashboardDto);
    }

    [Authorize]
    public async Task JoinDashboardGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, HubSettings.DashboardGroup);
        Console.WriteLine("New dashboard joined");
    }
}