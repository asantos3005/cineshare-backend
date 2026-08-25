using Microsoft.EntityFrameworkCore;
using cineshare_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace cineshare_backend.Data;


public class CineShareDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public CineShareDbContext(
        DbContextOptions<CineShareDbContext> options
    ) : base(options)
    {
    }

    public DbSet<Review> Reviews { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<MovieWatch> MovieWatches { get; set; }
    public DbSet<UserFollow> UserFollows { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // relationship and constraint configuration here
        modelBuilder.Entity<UserFollow>()
        .HasKey(f => new
        {
            f.FollowerId,
            f.FollowingId
        });
        
        modelBuilder.Entity<Like>()
        .HasKey(l => new
        {
            l.UserId,
            l.ReviewId
        });

        modelBuilder.Entity<MovieWatch>()
        .HasKey(mw => new
        {
            mw.UserId,
            mw.MovieId
        });

        modelBuilder.Entity<Movie>()
            .HasKey(m => m.InternalMovieId);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Movie)
            .WithMany(m => m.Reviews)
            .HasForeignKey(r => r.InternalMovieId);

        modelBuilder.Entity<UserFollow>()
            .HasOne(f => f.Follower)
            .WithMany(u => u.Following)
            .HasForeignKey(f => f.FollowerId);

        modelBuilder.Entity<UserFollow>()
            .HasOne(f => f.Following)
            .WithMany(u => u.Followers)
            .HasForeignKey(f => f.FollowingId);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        modelBuilder.Entity<Genre>()
            .HasIndex(g => g.GenreName)
            .IsUnique();

        modelBuilder.Entity<Movie>()
            .HasIndex(m => m.ExternalMovieId)
            .IsUnique();
    }
}
