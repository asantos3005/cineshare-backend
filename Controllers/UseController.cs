using Microsoft.AspNetCore.Mvc;
using cineshare_backend.Services;
using cineshare_backend.DTOs;
namespace cineshare_backend.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    private readonly ReviewService _reviewService;

    public UserController(UserService userService, ReviewService reviewService)
    {
        _userService = userService;
        _reviewService = reviewService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MyReviewResponse>>> GetReviews()
    {
        var reviews = await _reviewService.GetMyReviewsAsync(1);
        return Ok(reviews);
    }
}
