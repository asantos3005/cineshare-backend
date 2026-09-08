namespace cineshare_backend.Services;
using Microsoft.EntityFrameworkCore;
using cineshare_backend.Models;
using cineshare_backend.Data;
using cineshare_backend.DTOs;

public class UserService
{
    private readonly CineShareDbContext _db;
    private readonly MovieService _movieService;


    public UserService(CineShareDbContext db, MovieService movieService)
    {
        _db = db;
        _movieService = movieService;
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

    public async Task<ProfileResponse?> EditProfileAsync(
        int userId,
        EditProfileRequest request,
        IReadOnlySet<string> providedFields)
    {
        var user = await _db.Users
            .Include(u => u.FavoriteGenres)
            .Include(u => u.FourFavouriteMovies)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            return null;
        }

        if (providedFields.Contains(nameof(EditProfileRequest.FirstName)) &&
            request.FirstName is not null)
        {
            user.FirstName = request.FirstName;
        }

        if (providedFields.Contains(nameof(EditProfileRequest.LastName)) &&
            request.LastName is not null)
        {
            user.LastName = request.LastName;
        }

        if (providedFields.Contains(nameof(EditProfileRequest.ProfilePictureUrl)))
        {
            user.ProfilePictureUrl = request.ProfilePictureUrl;
        }

        if (providedFields.Contains(nameof(EditProfileRequest.Bio)))
        {
            user.Bio = request.Bio;
        }

        if (providedFields.Contains(nameof(EditProfileRequest.Genres)) &&
            request.Genres is not null)
        {
            var genreNames = request.Genres
                .Where(genre => !string.IsNullOrWhiteSpace(genre))
                .Select(genre => genre.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var genres = await _db.Genres
                .Where(genre => genreNames.Contains(genre.GenreName))
                .ToListAsync();

            if (genres.Count != genreNames.Count)
            {
                return null;
            }

            user.FavoriteGenres.Clear();

            foreach (var genre in genres)
            {
                user.FavoriteGenres.Add(genre);
            }
        }

        if (providedFields.Contains(nameof(EditProfileRequest.FourFavouriteMovies)) &&
            request.FourFavouriteMovies is not null)
        {
            var movieRequests = request.FourFavouriteMovies
                .DistinctBy(movie => movie.MovieId is not null
                    ? $"internal:{movie.MovieId}"
                    : $"external:{movie.ExternalMovieId}")
                .ToList();

            if (movieRequests.Count > 4)
            {
                return null;
            }

            var movies = new List<Movie>();

            foreach (var movieRequest in movieRequests)
            {
                var movie = movieRequest.MovieId is not null
                    ? await _db.Movies.FirstOrDefaultAsync(movie =>
                        movie.InternalMovieId == movieRequest.MovieId)
                    : null;

                if (movie is null && !string.IsNullOrWhiteSpace(movieRequest.ExternalMovieId))
                {
                    movie = await _movieService.GetInternalMovieByExternalIdAsync(
                        movieRequest.ExternalMovieId);

                    if (movie is null)
                    {
                        movie = await _movieService.FetchAndCreateNewInternalMovieAsync(
                            movieRequest.ExternalMovieId);
                    }
                }

                if (movie is null)
                {
                    return null;
                }

                movies.Add(movie);
            }

            user.FourFavouriteMovies ??= [];
            user.FourFavouriteMovies.Clear();

            foreach (var movie in movies)
            {
                user.FourFavouriteMovies.Add(movie);
            }
        }

        await _db.SaveChangesAsync();

        return await GetProfileByUserIdAsync(userId);
    }

}
