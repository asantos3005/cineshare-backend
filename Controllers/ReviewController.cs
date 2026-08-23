using Microsoft.AspNetCore.Mvc;
using cineshare_backend.Services;
using cineshare_backend.DTOs;
namespace cineshare_backend.Controllers;

[ApiController]
[Route("api/reviews")]
public class ReviewController : ControllerBase
{
    private readonly ReviewService _reviewService;

    public ReviewController(ReviewService reviewService)
    {
        _reviewService = reviewService;
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

    [HttpPost]
    public async Task<ActionResult<ReviewResponse>> CreateReview([FromBody] ReviewRequest reviewRequest)
    {
        var review = await _reviewService.CreateReviewAsync(reviewRequest);

        return CreatedAtAction(nameof(GetSpecificReview), new { id = review.Id }, review);
    }

}
