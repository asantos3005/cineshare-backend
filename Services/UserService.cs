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

    public Task<ProfileResponse?> GetProfileByUsernameAsync(string username)
    {
        return _db.Users
            .Where(u => u.UserName == username)
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

    public async Task<bool?> IsFollowingUserAsync(int currentUserId, string usernameToCheck)
    {
        var profileUser = await _db.Users
            .FirstOrDefaultAsync(u => u.UserName == usernameToCheck);

        if (profileUser is null)
        {
            return null;
        }

        return await _db.UserFollows.AnyAsync(f =>
            f.FollowerId == currentUserId &&
            f.FollowingId == profileUser.Id
        );
    }

    public async Task<bool> FollowUserAsync(int currentUserId, string usernameToFollow)
    {
        var userToFollow = await _db.Users
            .FirstOrDefaultAsync(u => u.UserName == usernameToFollow);

        if (userToFollow is null || userToFollow.Id == currentUserId)
        {
            return false;
        }

        var alreadyFollowing = await _db.UserFollows.AnyAsync(f =>
            f.FollowerId == currentUserId &&
            f.FollowingId == userToFollow.Id
        );

        if (alreadyFollowing)
        {
            return true;
        }

        _db.UserFollows.Add(new UserFollow
        {
            FollowerId = currentUserId,
            FollowingId = userToFollow.Id
        });

        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UnfollowUserAsync(int currentUserId, string usernameToUnfollow)
    {
        var userToUnfollow = await _db.Users
            .FirstOrDefaultAsync(u => u.UserName == usernameToUnfollow);

        if (userToUnfollow is null || userToUnfollow.Id == currentUserId)
        {
            return false;
        }

        var existingFollow = await _db.UserFollows.FirstOrDefaultAsync(f =>
            f.FollowerId == currentUserId &&
            f.FollowingId == userToUnfollow.Id
        );

        if (existingFollow is null)
        {
            return true;
        }

        _db.UserFollows.Remove(existingFollow);

        await _db.SaveChangesAsync();

        return true;
    }
}
