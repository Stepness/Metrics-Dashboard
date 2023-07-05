const serviceDomain = "https://metrics-monitoring-server.azurewebsites.net"

function login() {
  var usernameInput = document.getElementById('username');
  var passwordInput = document.getElementById('password');
  var errorMessage = document.getElementById('error-message');

  var username = usernameInput.value;
  var password = passwordInput.value;

  var requestBody = {
    username: username,
    password: password
  };

  fetch(`${serviceDomain}/users/login`, {
    method: 'POST',
    body: JSON.stringify(requestBody)
  })
    .then(function(response) {
      if (response.ok) {
        localStorage.setItem("jwtToken", response.body)
      } 
      else {
        errorMessage.textContent = 'Invalid username or password';
      }
    })
    .catch(function(error) {
      console.error('An error occurred during login:', error);
    });
}

function validate() {
  var jwtToken = localStorage.getItem('jwtToken');

  fetch(`${serviceDomain}/users/validate`, {
    method: 'POST',
    headers: {
      'Authorization': 'Bearer ' + jwtToken 
    }
  })
    .then(function(response) {
      if (response.ok) {
        if (window.location.href.includes('/login.html')) {
          window.location.href = '/index.html';
        }
      } else {
        if (!window.location.href.includes('/login.html'))
          window.location.href = '/login.html';
      }
    })
    .catch(function(error) {
      console.error('An error occurred during validation:', error);
    });
}
