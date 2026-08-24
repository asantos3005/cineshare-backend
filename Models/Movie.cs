namespace cineshare_backend.Models;

public class Movie
{
    public int InternalMovieId { get; set; }

    public string ExternalMovieId { get; set; } = "";

    public string Title { get; set; } = "";

    public string PosterUrl { get; set; } = "";

    public int ReleaseYear { get; set; } = 0;

    public ICollection<Review> Reviews { get; set; } = [];

    public ICollection<MovieWatch> WatchHistory { get; set; } = [];
}