namespace CityShare.Backend.Application.Core.Dtos.Authentication.Register;

public record RegisterRequestDto(
    string Email,
    string UserName,
    string Password);
