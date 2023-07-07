async function indexPageLoad(){
  await validateUser();
  await checkRoleClaim();
}

async function checkRoleClaim() {
  var jwtToken = getTokenBody();
  
  if (jwtToken) {
    var role = jwtToken.Role;
    var name = jwtToken.sub;
    
    fillUserMenu(role, name);
    
    if (role === 'Admin' || role === 'Viewer') {
      $('#index-error-message').text('Looks like there are no active machines.');
      await startSignalR();
      healthCheck();
    } 
    else{
      $('#index-error-message').text('You dont have enough permissions. Contact an administrator.')
    }

  } 
}


let connection = new signalR.HubConnectionBuilder()
//.withUrl("https://localhost:8080/hubs/metrics", {
.withUrl("https://metrics-monitoring-server.azurewebsites.net/hubs/metrics", {
accessTokenFactory: () => {
  return localStorage.getItem('jwtToken');
}
})
.configureLogging(signalR.LogLevel.Information)
.build();

const lastReceivedDataTimestamps = new Map();

function healthCheck(){ 
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
}

async function startSignalR(){
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
      if($('#index-error-message'))
        $('#index-error-message').text("")
      lastReceivedDataTimestamps.set(dashboardDto.connectionId, Date.now());
      generateOrUpdateCard(dashboardDto)
  })

  await connection.onclose(async () => {
      await start();
  });

  await start();
}
