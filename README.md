# Metrics Dashboard 

Metrics dashboard is a service based on a docker image that retrieves the metrics of the container in which is running.

Is capable of retrieving automatically CPU name, CPU usage, RAM usage, disk free, and Eth0 outgoing bytes.

The values of these images are shown on a dashboard online.

## Architecture
The service follows the Hub and spoke architecture, where the Hub service is the central point of connection.

![Architecture](/docs/metrics-dashboard-architecture.jpg)

The metrics collectors retrieve data and send them to the Hub via a WebSocket connection (SignalR), these messages are redirected to all authorized dashboard clients.

## Authentication and Authorization

### Login/Register

![Login](/docs/dashboard-login.png)

The passwords are one way hashed (SHA256) on a Db (CosmosDb).
The authorization on the dashboard and the Hub is managed via JWT.

When you register/login a request to the hub is made, if is successful a JWT token is returned and stored in the client.

The JWT contains the subject (username) and the role of the user.

Depending on the role different actions are available.

### Logout

On logout the JWT token gets deleted from the cache and are forced to login/register again.

![Logout](/docs/dashboard-logout.png)

## Roles
The roles supported by the service are:
- Guest. Just registered, it doesnt have permission to see metrics.
- Viewer. Promoted by an Admin from the Guest role, can see the metrics.
- Admin. Is the only one that can access the page to manage users roles.

The page to manage the users contains the usernames and their role.
Only Guest can be promoted to Viewer.

![Manage users](/docs/manage-users.png)

## Metrics

On the metrics page you see a card for each container running the metrics collector app.

You can see CPU name of the machine in which the container is running.

Data shown:
- CPU Name. Name of the machine in which the container is running
- CPU Usage
- Ram Usage. Expressed in MegaBytes
- Disk free. Expressed in percentage
- Eth0 out. Bytes exited from the container

The data shown are updated automatically. If no data is received from the application for 3 seconds the status change from healthy to unhealty, as shown below.

![Metrics](/docs/metrics.gif)

## Run the client
__Run on Windows__
```bash
docker run -d stepness/remote-logger
```

__Run on MacOS__
```bash
cpuName=$(sysctl -n machdep.cpu.brand_string)
docker run -de "CPU_NAME=$cpuName" stepness/remote-logger
```
