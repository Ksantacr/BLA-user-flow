using BLA.UserFlow.Application.DTO;
using BLA.UserFlow.Application.Services.TokenHandler;
using BLA.UserFlow.Application.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace BLA.UserFlow.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenHandler _tokenHandler;

    public AuthController(IUserService userService, ITokenHandler tokenHandler)
    {
        _userService = userService;
        _tokenHandler = tokenHandler;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto model)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var result = await _userService.RegisterUserAsync(model);

        if (result.Succeeded)
            return Ok("Successfully registered.");
        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userService.GetUserByEmailAsync(model.Email);

        if (user is null)
            return Unauthorized("Invalid credentials");

        if (!await _userService.IsValidPassword(model.Email, model.Password))
        {
            return Unauthorized("Invalid credentials");
        }

        // Generate JWT token
        var jwtToken = _tokenHandler.CreateJWTToken(user);
        return Ok(new { Token = jwtToken });
    }
}