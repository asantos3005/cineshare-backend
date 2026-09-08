namespace cineshare_backend.Services;
using Microsoft.EntityFrameworkCore;
using cineshare_backend.Models;
using cineshare_backend.Data;
using cineshare_backend.DTOs;

public class GenreService
{
    private readonly CineShareDbContext _db;

    public GenreService(CineShareDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<GenreResponseDTO>> getAllGenres()
    {
        return await _db.Genres
            .Select(g => new GenreResponseDTO(
                g.GenreId,
                g.GenreName
            ))
            .ToListAsync();

    }
}