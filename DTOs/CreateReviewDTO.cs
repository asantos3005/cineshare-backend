namespace cineshare_backend.DTOs;
public record CreateReviewRequest(
    int UserId,
    int MovieId,
    string Title,
    string ReviewBody,
    int Rating
);