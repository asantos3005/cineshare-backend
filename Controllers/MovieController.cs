using Microsoft.AspNetCore.Mvc;
using cineshare_backend.Services;
using cineshare_backend.Models;
using cineshare_backend.DTOs;
namespace cineshare_backend.Controllers;

[ApiController]
[Route("api/movies")]
public class MovieController : ControllerBase
{
    private readonly MovieService _movieService;

    public MovieController(MovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet("{externalMovieId}")]
    public async Task<ActionResult<Movie>> GetMovieByExternalId(string externalMovieId)
    {
        var movie = await _movieService.GetMovieByExternalIdAsync(externalMovieId);

        if (movie == null)
        {
            return NotFound();
        }

        return Ok(movie);
    }

    [HttpGet("/search={searchKeyword}")]
    public async Task<ActionResult<IEnumerable<OmdbSearchResponse>>> SearchMoviesExternal(string searchKeyword)
    {
        var movies = await _movieService.SearchMoviesExternalAsync(searchKeyword);

        if (movies == null)
        {
            return NotFound();
        }

        return Ok(movies);
    }   

}