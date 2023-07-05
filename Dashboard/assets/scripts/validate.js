function validate() {
  var jwtToken = localStorage.getItem('jwtToken');
  if (!jwtToken && window.location.href.includes('/login.html'))
    return;

  fetch("https://metrics-monitoring-server.azurewebsites.net/users/validate", {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + jwtToken 
    }
  })
    .then(function(response) {
      if (response.ok) {
        if (window.location.href.includes('/login.html')) {
          window.location.href = './index.html';
        }
      } else {
        if (!window.location.href.includes('/login.html'))
          window.location.href = './login.html';
      }
    })
    .catch(function(error) {
      console.error('An error occurred during validation:', error);
    });
}
  