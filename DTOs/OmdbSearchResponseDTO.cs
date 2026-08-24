namespace cineshare_backend.DTOs;
using System.Text.Json.Serialization;

public record OmdbSearchResponse(
    string ExternalMovieId,
    string Title,
    string Year,
    string? PosterUrl
);

public record OmdbSearchApiResponse(
    List<OmdbSearchApiMovie>? Search,
    string Response,
    string? Error
);

public record OmdbSearchApiMovie(
    string Title,
    string Year,
    [property: JsonPropertyName("imdbID")] string ExternalMovieId,
    string Type,
    [property: JsonPropertyName("Poster")] string? PosterUrl
);
