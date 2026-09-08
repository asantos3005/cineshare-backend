namespace cineshare_backend.DTOs;

public record EditProfileRequest(
    string? FirstName,
    string? LastName,
    string? ProfilePictureUrl,
    string? Bio,
    List<string>? Genres,
    List<EditProfileMovieRequest>? FourFavouriteMovies
);

public record EditProfileMovieRequest(
    int? MovieId,
    string? ExternalMovieId
);
