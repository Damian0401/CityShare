namespace CityShare.Backend.Application.Core.Dtos.Authentication.Login;

public record LoginResponseDto(UserDto User, string RefreshToken)
{
    public void Deconstruct(out UserDto user, out string refreshToken)
    {
        user = User;
        refreshToken = RefreshToken;
    }
}
