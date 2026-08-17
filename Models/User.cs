namespace cineshare_backend.Models;

public class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = "";

    public string Email { get; set; } = "";

    public string PasswordHash { get; set; } = "";

    public string ProfilePictureUrl { get; set; } = "";

    public string Bio { get; set; } = "";

    public ICollection<Genre> FavoriteGenres { get; set; } = [];

    public ICollection<Review> Reviews { get; set; } = [];

    public ICollection<Like> Likes { get; set; } = [];

    public ICollection<Movie> WatchedMovies { get; set; } = [];

    public ICollection<User> Followers { get; set; } = [];

    public ICollection<User> Following { get; set; } = [];
}