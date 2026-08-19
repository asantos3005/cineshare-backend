namespace cineshare_backend.Services;
using Microsoft.EntityFrameworkCore;
using cineshare_backend.Data;
using cineshare_backend.DTOs;

public class ReviewService
{
    private readonly CineShareDbContext _db;

    public ReviewService(CineShareDbContext db)
    {
        _db = db;
    }

    public Task<List<ReviewResponse>> GetReviewsAsync()
{
    return _db.Reviews
        .OrderByDescending(r => r.CreatedAt)
        .Select(r => new ReviewResponse(
            r.ReviewId,
            r.UserId,
            r.User.Username,
            r.MovieId,
            r.Movie.Title,
            r.Title,
            r.ReviewBody,
            r.Rating,
            r.CreatedAt
        ))
        .ToListAsync();
    }

    public Task<ReviewResponse?> GetReviewByIdAsync(int reviewId)
    {
        return _db.Reviews
            .Where(r => r.ReviewId == reviewId)
            .Select(r => new ReviewResponse(
                r.ReviewId,
                r.UserId,
                r.User.Username,
                r.MovieId,
                r.Movie.Title,
                r.Title,
                r.ReviewBody,
                r.Rating,
                r.CreatedAt
            ))
            .FirstOrDefaultAsync();
    }
   

}