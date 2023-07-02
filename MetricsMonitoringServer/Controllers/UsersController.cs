using System.ComponentModel.DataAnnotations;
using MetricsMonitoringServer.Services;
using Microsoft.AspNetCore.Authorization;

namespace MetricsMonitoringServer.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private IRepository _userService;

    public UsersController(IRepository userService)
    {
        _userService = userService;
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody]AuthenticateModel model)
    {
        var user = await _userService.Authenticate(model.Username, model.Password);

        if (user == null)
            return BadRequest(new { message = "Username or password is incorrect" });

        return Ok(user);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetUserNames();
        return Ok(users);
    }
}

public class AuthenticateModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}