namespace CityShare.Backend.Application.Core.Dtos.Auth;

public record AuthResponseDto(UserDto User, string RefreshToken);
