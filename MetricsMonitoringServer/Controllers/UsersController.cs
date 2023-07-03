using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MetricsMonitoringServer.Identity;
using MetricsMonitoringServer.Models;
using MetricsMonitoringServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace MetricsMonitoringServer.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IRepository _repository;

    public UsersController(IRepository repository)
    {
        _repository = repository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var user = await _repository.Authenticate(model.Username, model.Password);

        if (user == null)
            return BadRequest(new { message = "Username or password is incorrect" });

        return Ok(JwtTokenHelper.GenerateJsonWebToken(user));
    }

    [Authorize(Policy = IdentityData.ViewerUserPolicyName)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _repository.GetUserNames();
        return Ok(users);
    }


}
