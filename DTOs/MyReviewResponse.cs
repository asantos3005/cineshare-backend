namespace cineshare_backend.DTOs;

/*
When creating a new review, the client needs to send the external movie ID - 
the internal movie ID is only created afterwards, when the movie is added to the database. 
The external movie ID is used to fetch the movie details from the external API and create a new movie 
entry in the database if it doesn't exist yet.
*/
public record MyReviewResponse(
    int ReviewId,
    int UserId,
    int InternalMovieId,
    string MovieTitle,
    int MovieReleaseYear,
    string PosterUrl,
    string Title,
    string ReviewBody,
    int Rating,
    DateTime CreatedAt
);