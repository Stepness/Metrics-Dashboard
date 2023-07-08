async function usersPageLoad(){
  await validateUser();

  var jwtToken = getTokenBody();
  
  var role = jwtToken.Role;
  var name = jwtToken.sub;

  if(role != "Admin"){
    window.location.href = './index.html'
  }

  fillUserMenu(role, name);
  await generateUserTable();
}

async function generateUserTable() {
  var users = await getUsers();

  users.forEach(function (user) {
    var listItem = $(`<tr><td>${user.username}</td><td>${user.role}</td><td></td></tr>`);
    var userButton = $('<button></button>').text(user.username);

    userButton.click(function () {
    });

    listItem.find('td:last-child').append(userButton);
    $('#user-table').append(listItem);
  });
}


async function getUsers(userName) {
  var jwtToken = localStorage.getItem('jwtToken');

  var users = await $.ajax({
    url: 'https://metrics-monitoring-server.azurewebsites.net/users/',
    type: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + jwtToken
    },
    data: JSON.stringify({ name: userName }),
    contentType: 'application/json',
    success: function(response) {
    },
    error: function(error) {
      console.error('An error occurred when retrieving users', error)
    }
  });

  return users;
}

async function promoteToViewer(){

}