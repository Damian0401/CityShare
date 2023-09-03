namespace CityShare.Backend.Application.Core.Dtos.Authentication.Register;

public record RegisterResponseDto(UserDto User, string RefreshToken)
{
    public void Deconstruct(out UserDto user, out string refreshToken)
    {
        user = User;
        refreshToken = RefreshToken;
    }
}
