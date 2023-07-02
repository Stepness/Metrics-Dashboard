
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
    <div class="connection-id">
    <h2 style="display: inline;">${data.connectionId}</h2>
    <div class="healthy" id="${data.connectionId}-health"></div>
    </div>
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

function setUnhealthy(connectionId){
  const card = document.getElementById(`${connectionId}-health`);
  card.classList.add("unhealthy")
  card.classList.remove("healthy")
}
