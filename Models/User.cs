namespace cineshare_backend.Models;
using Microsoft.AspNetCore.Identity;


public class User : IdentityUser<int>
{
    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public string? ProfilePictureUrl { get; set; }

    public string? Bio { get; set; }

    public ICollection<Genre> FavoriteGenres { get; set; } = [];

    public ICollection<Movie>? FourFavouriteMovies { get; set; }

    public ICollection<Review> Reviews { get; set; } = [];

    public ICollection<Like> Likes { get; set; } = [];

    public ICollection<MovieWatch> WatchHistory { get; set; } = [];

    public ICollection<UserFollow> Followers { get; set; } = [];

    public ICollection<UserFollow> Following { get; set; } = [];
}
