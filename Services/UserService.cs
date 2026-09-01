namespace cineshare_backend.Services;
using Microsoft.EntityFrameworkCore;
using cineshare_backend.Models;
using cineshare_backend.Data;
using cineshare_backend.DTOs;

public class UserService
{
    private readonly CineShareDbContext _db;


    public UserService(CineShareDbContext db)
    {
        _db = db;
    }

    public Task<ProfileResponse?> GetProfileByUserIdAsync(int userId)
    {
        return _db.Users
            .Where(u => u.Id == userId)
            .Select(u => new ProfileResponse(
                u.FirstName,
                u.LastName,
                u.UserName ?? "",
                u.ProfilePictureUrl,
                u.Bio,
                new ProfileStatsResponse(
                    u.Reviews.Count,
                    u.WatchHistory.Count,
                    u.Followers.Count,
                    u.Following.Count
                ),
                u.FavoriteGenres
                    .Select(g => g.GenreName)
                    .ToList(),
                u.FourFavouriteMovies!
                    .Select(m => new ProfileMovieResponse(
                        m.InternalMovieId,
                        m.Title,
                        m.PosterUrl
                    ))
                    .ToList()
            ))
            .FirstOrDefaultAsync();
    }
}
