namespace cineshare_backend.DTOs;
using System.Text.Json.Serialization;

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
    [property: JsonPropertyName("movieId")]
    int InternalMovieId,
    string Title,
    string PosterUrl
);
