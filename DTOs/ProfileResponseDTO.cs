namespace cineshare_backend.DTOs;

public record ProfileResponse(
    string FirstName,
    string LastName,
    string Username,
    string? ProfilePictureUrl,
    string? Bio,
    ProfileStatsResponse Stats,
    List<string> Genres,
    List<ProfileMovieResponse> FourFavouriteMovies
);

public record ProfileStatsResponse(
    int ReviewCount,
    int MovieCount,
    int FollowerCount,
    int FollowingCount
);

public record ProfileMovieResponse(
    int InternalMovieId,
    string Title,
    string PosterUrl
);