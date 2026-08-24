namespace cineshare_backend.Services;
using Microsoft.EntityFrameworkCore;
using cineshare_backend.Models;
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
                r.User.ProfilePictureUrl,
                r.MovieId,
                r.Movie.Title,
                r.Movie.ReleaseYear,
                r.Movie.PosterUrl,
                r.Title,
                r.ReviewBody,
                r.Rating,
                r.CreatedAt,
                r.Likes.Count
            ))
            .ToListAsync();
    }


    /*
    This service gets review by internal review ID. 
    The internal review ID is generated when the review is created and stored in the database. 
    Different from the external movie ID, which is used to fetch movie details from an external API, 
    the internal review ID is unique to each review and is used to identify and manage 
    reviews within the application.
    */
    public Task<ReviewResponse?> GetReviewByIdAsync(int reviewId)
    {
        return _db.Reviews
            .Where(r => r.ReviewId == reviewId)
            .Select(r => new ReviewResponse(
                r.ReviewId,
                r.UserId,
                r.User.Username,
                r.User.ProfilePictureUrl,
                r.MovieId,
                r.Movie.Title,
                r.Movie.ReleaseYear,
                r.Movie.PosterUrl,
                r.Title,
                r.ReviewBody,
                r.Rating,
                r.CreatedAt,
                r.Likes.Count
            ))
            .FirstOrDefaultAsync();
    }

    public async Task<ReviewResponse?> CreateReviewAsync(CreateReviewRequest request)
    {
        var review = new Review
        {
            UserId = request.UserId,
            ExternalMovieId = request.ExternalMovieId,
            Title = request.Title,
            ReviewBody = request.ReviewBody,
            Rating = request.Rating
        };

        _db.Reviews.Add(review);

        await _db.SaveChangesAsync();

        return await GetReviewByIdAsync(review.ReviewId);
    }

    public async Task<ReviewResponse?> UpdateReviewAsync(int reviewId, UpdateReviewRequest request)
    {
        var review = await _db.Reviews.FindAsync(reviewId);

        if (review == null)
        {
            return null;
        }

        review.Title = request.Title;
        review.ReviewBody = request.ReviewBody;
        review.Rating = request.Rating;

        await _db.SaveChangesAsync();

        return await GetReviewByIdAsync(review.ReviewId);
    }

    public async Task<bool> DeleteReviewAsync(int reviewId)
    {
        var review = await _db.Reviews.FindAsync(reviewId);

        if (review == null)
        {
            return false;
        }

        _db.Reviews.Remove(review);
        await _db.SaveChangesAsync();

        return true;
    }
   

}