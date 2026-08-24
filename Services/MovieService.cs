namespace cineshare_backend.Services;
using Microsoft.EntityFrameworkCore;
using cineshare_backend.Models;
using cineshare_backend.Data;
using cineshare_backend.DTOs;

public class MovieService
{
    private readonly CineShareDbContext _db;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public MovieService(CineShareDbContext db, IConfiguration configuration, HttpClient httpClient)
    {
        _db = db;
        _configuration = configuration;
        _httpClient = httpClient;
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
    }

    public async Task<Movie?> GetMovieByExternalIdAsync(string externalMovieId)
    {
        return await _db.Movies.FirstOrDefaultAsync(m => m.ExternalMovieId == externalMovieId);
    }

    /*
    Used by frontend client to search for movies by title. 
    This service calls the external OMDb API to fetch movie search results based on the provided title. 
    The results are returned as a list of OmdbSearchResponse objects, 
    which contain essential information about each movie, such as its external ID, title, year, and poster URL.
    */
    public async Task<List<OmdbSearchResponse>> SearchMoviesExternalAsync(string title)
    {
        var apiKey = GetOmdbApiKey();
        var encodedTitle = Uri.EscapeDataString(title);

        var url =
            $"https://www.omdbapi.com/?apikey={apiKey}&s={encodedTitle}&type=movie";

        var response = await _httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var searchResponse = await response.Content
            .ReadFromJsonAsync<OmdbSearchApiResponse>();

        if (searchResponse?.Search == null)
        {
            return [];
        }

        return searchResponse.Search
            .Select(movie => new OmdbSearchResponse(
                movie.ExternalMovieId,
                movie.Title,
                movie.Year,
                movie.PosterUrl == "N/A" ? null : movie.PosterUrl
            ))
            .ToList();
    }

    /*
    Private Utility Functions
    */
    private async Task<OmdbMovieResponse?> FetchMovieDetailsFromExternalApiAsync(
    string externalMovieId)
    {
        var apiKey = _configuration["Omdb:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Missing OMDb API key. Set it with: dotnet user-secrets set \"Omdb:ApiKey\" \"your-api-key\"");
        }

        var url =
            $"https://www.omdbapi.com/?i={externalMovieId}&apikey={apiKey}";

        var response = await _httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var movie = await response.Content
            .ReadFromJsonAsync<OmdbMovieResponse>();

        return movie;
    }

    private string GetOmdbApiKey()
    {
        var apiKey = _configuration["Omdb:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Missing OMDb API key. Set it with: dotnet user-secrets set \"Omdb:ApiKey\" \"your-api-key\"");
        }

        return apiKey;
    }

}
