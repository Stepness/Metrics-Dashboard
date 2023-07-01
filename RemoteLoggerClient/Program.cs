using Microsoft.AspNetCore.SignalR.Client;
using RemoteLoggerClient;
using RemoteLoggerClient.Models;

Console.WriteLine($"New {Metrics.GetTransmittedBytesForEth0()}");

var connectionId = Environment.GetEnvironmentVariable("HOSTNAME");

Console.WriteLine("Connection starting...");

var connection = new HubConnectionBuilder()
    //.WithUrl("http://host.docker.internal:5200/hubs/metrics")
    .WithUrl("https://metrics-monitoring-server.azurewebsites.net/hubs/metrics")
    .WithAutomaticReconnect()
    .Build();

connection.Closed += async (error) =>
{
    await Task.Delay(new Random().Next(0,5) * 1000);
    await connection.StartAsync();
};

await connection.StartAsync();
Console.WriteLine("Connection started");

await connection.InvokeAsync("JoinClientGroup");
Console.WriteLine("Joined client group");

while (true)
{
    var metrics = new MetricsDto()
    {
        RamUsageMegabytes = Metrics.GetRamUsageMegabytes(),
        CPUName = await Metrics.GetCpuNameAsync(),
        DiskFreePercentage = Metrics.GetDiskSpacePercentage(),
        CPUUsagePercentage = await Metrics.GetCpuUsageAsync(),
        Eth0TransmittedBytes = Metrics.GetTransmittedBytesForEth0()
    };

    await connection.SendAsync("SendBroadcastMessage", connectionId, metrics);
    await Task.Delay(1000);
}