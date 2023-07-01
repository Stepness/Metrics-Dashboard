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
    updateCard(existingCard, data);
  } else {
    createCard(data);
  }
}

function updateCard(card, data) {
  card.innerHTML = generateCardHTML(data);
}

function createCard(data) {
  const cardContainer = document.getElementById('metrics-container');
  const card = document.createElement('div');
  card.id = data.connectionId;
  card.className = 'card';
  card.innerHTML = generateCardHTML(data);
  cardContainer.appendChild(card);
}

function generateCardHTML(data) {
  return `
    <h2>${data.connectionId}</h2>
    <div>
      <span class="label">CPU Name:</span>
      <span class="data">${data.cpuName}</span>
    </div>
    <div>
      <span class="label">CPU Usage:</span>
      <span class="data">${data.cpuUsagePercentage}%</span>
    </div>
    <div>
      <span class="label">Ram Usage:</span>
      <span class="data">${data.ramUsageMegabytes} MB</span>
    </div>
    <div>
      <span class="label">Disk free:</span>
      <span class="data">${data.diskFreePercentage}%</span>
    </div>
    <div>
      <span class="label">Eth0 out:</span>
      <span class="data">${data.eth0TransmittedBytes.toLocaleString()} Bytes</span>
    </div>
  `;
}