namespace cineshare_backend.DTOs;

public record UpdateReviewRequest(
    string Title,
    string ReviewBody,
    int Rating
);