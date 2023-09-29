namespace CityShare.Backend.Application.Core.Dtos.Auth;

public record RegisterRequestDto(
    string Email,
    string UserName,
    string Password);
