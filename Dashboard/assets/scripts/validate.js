async function validateUser() {
  var jwtToken = localStorage.getItem('jwtToken');
  if (!jwtToken && window.location.href.includes('/login.html'))
    return;
  else if (!jwtToken){
    window.location.href = './login.html'
    return
  }

  await $.ajax({
    url: 'https://metrics-monitoring-server.azurewebsites.net/users/validate',
    type: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + jwtToken
    },
    success: function(response) {
      if (window.location.href.includes('/login.html')) {
        window.location.href = './index.html';
      }
    },
    error: function(xhr, status, error) {
      if (!window.location.href.includes('/login.html')) {
        window.location.href = './login.html';
      }
      console.error('An error occurred during validation:', error);
    }
  });
}
