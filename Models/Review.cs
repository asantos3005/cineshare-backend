namespace cineshare_backend.Models;
public class Review
{
    public int ReviewId { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int InternalMovieId { get; set; }
    public Movie Movie { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string Title { get; set; } = "";

    public string ReviewBody { get; set; } = "";

    public int Rating { get; set; }

    public ICollection<Like> Likes { get; set; } = [];
}