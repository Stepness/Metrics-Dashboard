function usersPageLoad(){
  validateUser();

  var jwtToken = getTokenBody();
  
  var role = jwtToken.Role;
  var name = jwtToken.sub;

  if(role != "Admin"){
    window.location.href = './index.html'
  }

  fillUserMenu(role, name);
}