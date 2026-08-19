using Microsoft.AspNetCore.Mvc;
using cineshare_backend.Services;
using Microsoft.EntityFrameworkCore;
using cineshare_backend.Data;
using cineshare_backend.Models;

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

}

public record CreateReviewRequest(
    int UserId,
    int MovieId,
    string Title,
    string ReviewBody,
    int Rating
);

public record UpdateReviewRequest(
    string Title,
    string ReviewBody,
    int Rating
);


