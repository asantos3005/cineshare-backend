namespace cineshare_backend.Services;
using Microsoft.EntityFrameworkCore;
using cineshare_backend.Models;
using cineshare_backend.Data;
using cineshare_backend.DTOs;

public class MovieService
{
    private readonly CineShareDbContext _db;

    public MovieService(CineShareDbContext db)
    {
        _db = db;
    }

    public async Task<bool> MovieExistsAsync(string externalMovieId)
    {
        var existingMovie = await _db.Movies.FirstOrDefaultAsync(m => m.ExternalMovieId == externalMovieId);

        if (existingMovie != null)
        {
            return true; // Movie already exists
        }

        return false; // Movie does not exist
    }

    public async Task<Movie> FetchAndCreateNewMovieInternalAsync(string externalMovieId)
    {
        // Fetch movie details from the external API using the externalMovieId
        var movieDetails = await FetchMovieDetailsFromExternalApiAsync(externalMovieId);

        if (movieDetails == null)
        {
            throw new Exception("Failed to fetch movie details from the external API.");
        }

        var movie = new Movie
        {
            ExternalMovieId = externalMovieId,
            Title = movieDetails.Title,
            PosterUrl = movieDetails.Poster,
            ReleaseYear = int.Parse(movieDetails.Year)
        };
    {
        _db.Movies.Add(movie);
        await _db.SaveChangesAsync();

        return movie;
    }

    public async Task<Movie?> GetMovieByExternalIdAsync(string externalMovieId)
    {
        return await _db.Movies.FirstOrDefaultAsync(m => m.ExternalMovieId == externalMovieId);
    }

    private async Task<OmdbMovieResponse?> FetchMovieDetailsFromExternalApiAsync(
    string externalMovieId)
{
    var apiKey = _configuration["Omdb:ApiKey"];

    var url =
        $"https://www.omdbapi.com/?apikey={apiKey}&i={externalMovieId}";

    var response = await _httpClient.GetAsync(url);

    response.EnsureSuccessStatusCode();

    var movie = await response.Content
        .ReadFromJsonAsync<OmdbMovieResponse>();

    return movie;
}

}