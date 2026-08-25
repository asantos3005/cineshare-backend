using Microsoft.AspNetCore.Mvc;
using cineshare_backend.Services;
using cineshare_backend.Models;
using cineshare_backend.DTOs;
namespace cineshare_backend.Controllers;
using Microsoft.AspNetCore.Identity;


[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _signInManager.PasswordSignInAsync(
            request.UserName,
            request.Password,
            isPersistent: true,
            lockoutOnFailure: false
        );

        if (!result.Succeeded)
        {
            return Unauthorized();
        }

        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
}