function login() {
  var username = $('#username').val().trim();
  var password = $('#password').val();
  var errorMessage = $('#error-message');

  if(!isLoginValid(username, password))
    return;

  var requestBody = {
    username: username,
    password: password
  };

  $.ajax({
    url: 'https://metrics-monitoring-server.azurewebsites.net/users/login',
    type: 'POST',
    dataType: 'text',
    contentType: 'application/json',
    data: JSON.stringify(requestBody),
    success: function(response) {
      localStorage.setItem('jwtToken', response);
      window.location.href = './index.html';
    },
    error: function(xhr, status, error) {
      errorMessage.text('Invalid username or password');
      console.error('An error occurred during login:', error);
    }
  });
}


async function register() {
  var username = $('#username').val().trim();
  var password = $('#password').val();
  var errorMessage = $('#error-message');

  if(!isLoginValid(username, password))
  return;
  
  var requestBody = {
    username: username,
    password: password
  };

  await $.ajax({
    url: 'https://metrics-monitoring-server.azurewebsites.net/users/register',
    type: 'POST',
    contentType: 'application/json',
    data: JSON.stringify(requestBody),
    success: function(response) {
      localStorage.setItem('jwtToken', response);
      window.location.href = './index.html';
    },
    error: function(xhr, status, error) {
      if(xhr.status === 409)
        errorMessage.text('User already exists');
      else
        console.error('An error occurred during login:', error);
    }
  });
}

function isLoginValid() {
  if (username === '' || password.value === '') {
    $("#error-message").text('Please enter a username and password.');
    return false;
  }

  return true;
}