namespace cineshare_backend.Models;

public class MovieWatch
{
    public int UserId { get; set; }
    public User WatchedBy { get; set; } = null!;

    public int MovieId { get; set; }
    public Movie WatchedMovie { get; set; } = null!;

    public DateTime WatchedAt { get; set; } = DateTime.UtcNow;
}