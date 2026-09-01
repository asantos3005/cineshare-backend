using Microsoft.AspNetCore.Mvc;
using cineshare_backend.Services;
using cineshare_backend.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using cineshare_backend.Models;
namespace cineshare_backend.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    private readonly ReviewService _reviewService;

    private readonly UserManager<User> _userManager;


    public UserController(UserService userService, ReviewService reviewService, UserManager<User> userManager)

    {
        _userService = userService;
        _reviewService = reviewService;
        _userManager = userManager;
    }

    // Get all of the current user's reviews
    [Authorize]
    [HttpGet("my-reviews")]
    public async Task<ActionResult<IEnumerable<MyReviewResponse>>> GetMyReviews()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Unauthorized();
        }

        var reviews = await _reviewService.GetMyReviewsAsync(user.Id);
        return Ok(reviews);
    }

    [Authorize]
    [HttpGet("my-profile")]
    public async Task<ActionResult<ProfileResponse>> GetMyProfile()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Unauthorized();
        }

        var profile = await _userService.GetProfileByUserIdAsync(user.Id);

        if (profile is null)
        {
            return NotFound();
        }

        return Ok(profile);
    }
}
