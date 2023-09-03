namespace CityShare.Backend.Application.Core.Dtos.Authentication.Login;

public record LoginResponseDto(UserDto User, string RefreshToken);