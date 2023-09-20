namespace CityShare.Backend.Application.Core.Dtos.Auth.Login;

public record LoginResponseDto(UserDto User, string RefreshToken);