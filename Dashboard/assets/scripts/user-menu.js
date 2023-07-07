function openUserMenu() {
  document.getElementById("userMenuDropDown").classList.toggle("show");
}

function fillUserMenu(role, name) {
  $('#userMenubtn').text(name);

  var userMenuDiv = $("#userMenuDropDown");
  
    if (role === "Admin"){
      var link = $("<a>").attr("href", "./users.html").text("Manage users");
      userMenuDiv.append(link)
    }

  var link = $("<a>").text("Logout");
  link.on("click", function() {
    localStorage.removeItem("jwtToken");
    window.location.href = './login.html';
  });

  userMenuDiv.append(link)
}

// Close the dropdown menu if the user clicks outside of it
window.onclick = function(event) {
  if (!event.target.matches('.dropbtn')) {
    var dropdowns = $(".dropdown-content");
    var i;
    for (i = 0; i < dropdowns.length; i++) {
      var openDropdown = dropdowns[i];
      if (openDropdown.classList.contains('show')) {
        openDropdown.classList.remove('show');
      }
    }
  }
}
