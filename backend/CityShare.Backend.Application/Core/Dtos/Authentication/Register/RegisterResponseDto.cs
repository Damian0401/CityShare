namespace CityShare.Backend.Application.Core.Dtos.Authentication.Register;

public record RegisterResponseDto(UserDto User, string RefreshToken);
