const connection = new signalR.HubConnectionBuilder()
    //.withUrl("http://localhost:5200/hubs/metrics")
    .withUrl("https://metrics-monitoring-server.azurewebsites.net/hubs/metrics")
    .configureLogging(signalR.LogLevel.Information)
    .build();

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
      generateOrUpdateCard(dashboardDto)
  })

  connection.onclose(async () => {
      await start();
  });

  start();    
}

function generateOrUpdateCard(data) {
  const existingCard = document.getElementById(data.connectionId);
    
  if (existingCard) {
    existingCard.innerHTML = `
      <h2>${data.connectionId}</h2>
      <div>
      <span class="label">CPU Usage:</span>
      <span class="data">${data.cpuUsagePercentage}%</span>
      </div>
      <div>
      <span class="label">Ram Usage:</span>
      <span class="data">${data.ramUsageMegabytes} Mb</span>
      </div>
    `;
  } else {
    const cardContainer = document.getElementById('metrics-container');
    const card = document.createElement('div');
    card.id = data.connectionId;
    card.className = 'card';
    card.innerHTML = `
      <h2>${data.connectionId}</h2>
      <div>
      <span class="label">CPU Usage:</span>
      <span class="data">${data.cpuUsagePercentage}%</span>
      </div>
      <div>
      <span class="label">Ram Usage:</span>
      <span class="data">${data.ramUsageMegabytes} Mb</span>
      </div>
    `;
    cardContainer.appendChild(card);
  }
}
  