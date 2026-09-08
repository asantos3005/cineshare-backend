using Microsoft.AspNetCore.Mvc;
using cineshare_backend.Services;
using cineshare_backend.Models;
using cineshare_backend.DTOs;
namespace cineshare_backend.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


[ApiController]
[Route("api/genre")]
public class GenreController : ControllerBase
{
    private readonly GenreService _genreService;
    public GenreController(GenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<GenreResponseDTO>>> GetAllGenres()
    {
        var genres = await _genreService.getAllGenres();
        return Ok(genres);
    }
}