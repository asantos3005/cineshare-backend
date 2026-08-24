namespace cineshare_backend.DTOs;
public record OmdbMovieResponse(
    string Title,
    string Year,
    string Poster,
    string ImdbID,
    string Response
);