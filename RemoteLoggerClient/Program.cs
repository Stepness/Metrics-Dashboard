using Microsoft.AspNetCore.SignalR.Client;
using RemoteLoggerClient;
using RemoteLoggerClient.Models;

var connectionId = Environment.GetEnvironmentVariable("HOSTNAME");

var connection = new HubConnectionBuilder()
    .WithUrl("http://host.docker.internal:5200/metricshub")
    .WithAutomaticReconnect()
    .Build();

connection.Closed += async (error) =>
{
    await Task.Delay(new Random().Next(0,5) * 1000);
    await connection.StartAsync();
};

await connection.StartAsync();

while (true)
{
    var metrics = new MetricsDto()
    {
        RamUsageMegabytes = Metrics.RamUsageMegabytes(),
        CPUName = await Metrics.CpuNameAsync(),
        DiskFreePercentage = Metrics.DiskSpace(),
        CPUUsagePercentage = Metrics.CpuUsage()
    };

    await connection.SendAsync("ReceiveMessage", connectionId, metrics);
    
    await Task.Delay(3000);
}