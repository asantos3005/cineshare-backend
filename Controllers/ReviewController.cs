using Microsoft.AspNetCore.Mvc;
using cineshare_backend.Services;
using cineshare_backend.DTOs;
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

}

public record UpdateReviewRequest(
    string Title,
    string ReviewBody,
    int Rating
);


