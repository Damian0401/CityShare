namespace CityShare.Backend.Application.Core.Dtos.Auth.Register;

public record RegisterRequestDto(
    string Email,
    string UserName,
    string Password);
