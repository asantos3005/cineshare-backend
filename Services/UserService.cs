namespace cineshare_backend.Services;
using Microsoft.EntityFrameworkCore;
using cineshare_backend.Models;
using cineshare_backend.Data;
using cineshare_backend.DTOs;

public class UserService
{
    private readonly CineShareDbContext _db;


    public UserService(CineShareDbContext db)
    {
        _db = db;
    }
}
