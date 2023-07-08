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
    var listItem = $(`<tr><td>${user.username}</td><td id="${user.username}-role">${user.role}</td><td></td</tr>`);

    if(user.role === 'Guest' ){
      var promoteButton = $('<button class="promote-button"></button>').text("Promote");
  
      promoteButton.click(async function () {
        $(this).prop('disabled', true); 
        
        if(await promoteToViewer(user.username)){
          $(`#${user.username}-role`).text("Viewer")
          $(this).remove();
        }else

        $(this).prop('disabled', false); 
      });
  
      listItem.find('td:last-child').append(promoteButton);
    }
    $('#user-table').append(listItem);
  });
}


async function getUsers(username) {
  var jwtToken = localStorage.getItem('jwtToken');

  var users = await $.ajax({
    url: 'https://metrics-monitoring-server.azurewebsites.net/users/',
    type: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + jwtToken
    },
    data: JSON.stringify({ name: username }),
    contentType: 'application/json',
    success: function(response) {
    },
    error: function(error) {
      console.error('An error occurred when retrieving users', error)
    }
  });

  return users;
}

async function promoteToViewer(username){
  var jwtToken = localStorage.getItem('jwtToken');
  var success = false;

  await $.ajax({
    url: `https://metrics-monitoring-server.azurewebsites.net/users/${username}/promote-role`,
    type: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + jwtToken
    },
    contentType: 'application/json',
    success: function(response) {
      success = true
    },
    error: function(xhr, status, error) {
      console.error('An error occurred during promotion:', error);
    }
  });

  return success;
}