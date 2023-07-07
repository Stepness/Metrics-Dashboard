async function indexPageLoad(){
  await validateUser();
  await checkRoleClaim();
}

async function checkRoleClaim() {
  var jwtToken = getTokenBody();
  
  if (jwtToken) {
    var role = jwtToken.Role;
    var name = jwtToken.sub;
    
    fillUserMenu(role, name);
    
    if (role === 'Admin' || role === 'Viewer') {
      $('#index-error-message').text('Looks like there are no active machines.');
      await startSignalR();
      healthCheck();
    } 
    else{
      $('#index-error-message').text('You dont have enough permissions. Contact an administrator.')
    }

  } 
}
  