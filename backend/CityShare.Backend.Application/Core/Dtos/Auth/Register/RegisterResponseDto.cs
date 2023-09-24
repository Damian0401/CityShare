namespace CityShare.Backend.Application.Core.Dtos.Auth.Register;

public record RegisterResponseDto(UserDto User, string RefreshToken);
