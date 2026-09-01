namespace cineshare_backend.DTOs;
public record RegisterRequest(
    string UserName,
    string Email,
    string Password,
    string FirstName,
    string LastName
);
