namespace cineshare_backend.DTOs;
public record ReviewResponse(
    int ReviewId,
    int UserId,
    string Username,
    string UserProfilePictureUrl,
    int MovieId,
    string MovieTitle,
    int MovieReleaseYear,
    string PosterUrl,
    string Title,
    string ReviewBody,
    int Rating,
    DateTime CreatedAt,
    int LikesCount
);
