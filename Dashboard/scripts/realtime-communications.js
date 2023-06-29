const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5200/hubs/metrics")
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
    })

    connection.onclose(async () => {
        await start();
    });

    // Start the connection.
    start();    
}