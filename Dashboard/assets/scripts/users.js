function usersPageLoad(){
  validateUser();

  var jwtToken = getTokenBody();
  
  var role = jwtToken.Role;
  var name = jwtToken.sub;

  if(role != "Admin"){
    window.location.href = './index.html'
  }

  fillUserMenu(role, name);
  generateUserTable();
}

function generateUserTable(){
  // User data (example)
  var users = [
    { name: 'User 1', id: 1 },
    { name: 'User 2', id: 2 },
    { name: 'User 3', id: 3 }
  ];

  users.forEach(function(user) {
    var listItem = $('<li></li>');
    var userButton = $('<button></button>').text(user.name);
    userButton.click(function() {
      // sendHttpRequest(user.name);
    });
    listItem.append(userButton);
    $('#user-list').append(listItem);
  });

  // Function to send HTTP request with user name
  function sendHttpRequest(userName) {
    // Replace this code with your actual HTTP request logic
    // Example using jQuery AJAX:
    $.ajax({
      url: 'http://example.com/api/users',
      method: 'POST',
      data: JSON.stringify({ name: userName }),
      contentType: 'application/json',
      success: function(response) {
        // Handle the response
      },
      error: function(error) {
        // Handle the error
      }
    });
  }
}