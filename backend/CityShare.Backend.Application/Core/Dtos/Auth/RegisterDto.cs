namespace CityShare.Backend.Application.Core.Dtos.Auth;

public record RegisterDto(
    string Email,
    string UserName,
    string Password);
