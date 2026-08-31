using Microsoft.AspNetCore.Mvc;
using cineshare_backend.Services;
using cineshare_backend.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using cineshare_backend.Models;
namespace cineshare_backend.Controllers;

[ApiController]
[Route("api/reviews")]
public class ReviewController : ControllerBase
{
    private readonly ReviewService _reviewService;
    private readonly UserManager<User> _userManager;

    public ReviewController(
        ReviewService reviewService,
        UserManager<User> userManager)
    {
        _reviewService = reviewService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReviewResponse>>> GetReviews()
    {
        var reviews = await _reviewService.GetReviewsAsync();

        return Ok(reviews);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReviewResponse>> GetSpecificReview(int id)
    {
        var review = await _reviewService.GetReviewByIdAsync(id);

        if (review == null)
        {
            return NotFound();
        }

        return Ok(review);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ReviewResponse>> CreateReview([FromBody] CreateReviewRequest reviewRequest)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Unauthorized();
        }

        var review = await _reviewService.CreateReviewAsync(reviewRequest, user.Id);

        if (review is null)
        {
            return Problem("The review was created, but the response could not be loaded.");
        }

        return CreatedAtAction(nameof(GetSpecificReview), new { id = review.ReviewId }, review);
    }

}
