const connection = new signalR.HubConnectionBuilder()
    //.withUrl("http://localhost:5200/hubs/metrics")
    .withUrl("https://metrics-monitoring-server.azurewebsites.net/hubs/metrics")
    .configureLogging(signalR.LogLevel.Information)
    .build();

const lastReceivedDataTimestamps = new Map();

setInterval(() => {
  const currentTimestamp = Date.now();
  lastReceivedDataTimestamps.forEach((date, connectionId) => {
    const lastReceivedTimestamp = date;
    const elapsedTime = currentTimestamp - lastReceivedTimestamp;
    
    if (elapsedTime >= 3000) {
      setUnhealthy(connectionId);
    }
  });
}, 1000);

function startSignalR(){
  async function start() {
      try {
          await connection.start();
          console.log("SignalR Connected.");
          await connection.invoke("JoinDashboardGroup");
      } catch (err) {
          console.log(err);
      }
  }

  connection.on("ReceiveBroadcastMessage", (dashboardDto) => {
      console.log(dashboardDto);
      lastReceivedDataTimestamps.set(dashboardDto.connectionId, Date.now());
      generateOrUpdateCard(dashboardDto)
  })

  connection.onclose(async () => {
      await start();
  });

  start();    
}