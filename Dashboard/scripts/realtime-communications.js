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

    // Start the connection.
    start();    
}

function generateOrUpdateCard(data) {
    // Check if card already exists with the given ID
    const existingCard = document.getElementById(data.connectionId);
    
    if (existingCard) {
      // Update the values of existing card
      existingCard.innerHTML = `
        <p>${data.cpuUsagePercentage}</p>
        <p>${data.ramUsageMegabytes}</p>
      `;
    } else {
      // Create a new card
      const cardContainer = document.getElementById('metrics-container');
      const card = document.createElement('div');
      card.id = data.connectionId;
      card.className = 'card';
      card.innerHTML = `
        <p>${data.cpuUsagePercentage}</p>
        <p>${data.ramUsageMegabytes}</p>
      `;
      cardContainer.appendChild(card);
    }
  }
  