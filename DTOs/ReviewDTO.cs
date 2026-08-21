namespace cineshare_backend.DTOs;
public record ReviewResponse(
    int ReviewId,
    int UserId,
    string Username,
    int MovieId,
    string MovieTitle,
    DateOnly? MovieReleaseDate,
    string PosterUrl,
    string Title,
    string ReviewBody,
    int Rating,
    DateTime CreatedAt
);
