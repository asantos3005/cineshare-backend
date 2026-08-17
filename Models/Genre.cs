namespace cineshare_backend.Models;

public class Genre
{
    public int GenreId { get; set; }

    public string GenreName { get; set; } = "";

    public ICollection<User> FavoritedByUsers { get; set; } = [];
}