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

  fetch("https://metrics-monitoring-server.azurewebsites.net/users/login", {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(requestBody)
  })
    .then(function(response) {
      if (response.ok) {
        return response.text();
      } 
      else {
        errorMessage.textContent = 'Invalid username or password';
      }
    })
    .then(function(jwtToken){
      localStorage.setItem('jwtToken', jwtToken);
      window.location.href = './index.html';
    })
    .catch(function(error) {
      console.error('An error occurred during login:', error);
    });
}
