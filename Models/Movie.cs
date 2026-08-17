namespace cineshare_backend.Models;

public class Movie
{
    public int MovieId { get; set; }

    public int ExternalMovieId { get; set; }

    public string Title { get; set; } = "";

    public string PosterUrl { get; set; } = "";

    public DateOnly? ReleaseDate { get; set; }

    public ICollection<Review> Reviews { get; set; } = [];

    public ICollection<MovieWatch> WatchHistory { get; set; } = [];
}