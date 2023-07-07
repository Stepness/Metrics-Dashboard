function login() {
  var usernameInput = $('#username');
  var passwordInput = $('#password');
  var errorMessage = $('#error-message');

  var username = usernameInput.val();
  var password = passwordInput.val();

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
  var usernameInput = $('#username');
  var passwordInput = $('#password');
  var errorMessage = $('#error-message');

  var username = usernameInput.val();
  var password = passwordInput.val();

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
