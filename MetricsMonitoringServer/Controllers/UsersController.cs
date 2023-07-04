using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MetricsMonitoringServer.Identity;
using MetricsMonitoringServer.Models;
using MetricsMonitoringServer.Services;
using MetricsMonitoringServer.Settings;
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

    [HttpPost("register")]
    public async Task<IActionResult> Register(SignDto signDto)
    {
        var user = new UserEntity
        {
            Username = signDto.Username,
            Password = signDto.Password,
            Role = Roles.Guest
        };
        
        var addUserResult = await _repository.AddUserAsync(user);

        if (addUserResult.Result == AddUserResultType.UserAlreadyExists)
            return Conflict(new { message = "Username already exist" });

        return Ok(JwtTokenHelper.GenerateJsonWebToken(addUserResult.User));
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(SignDto model)
    {
        var user = await _repository.Authenticate(model.Username, model.Password);

        if (user == null)
            return Unauthorized(new { message = "Username or password is incorrect" });

        return Ok(JwtTokenHelper.GenerateJsonWebToken(user));
    }

    [Authorize(Policy = IdentityData.AdminUserPolicy)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _repository.GetAllUsers();
        return Ok(users.Select(x => x.Username).ToList());
    }
}
