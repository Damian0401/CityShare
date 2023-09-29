namespace CityShare.Backend.Application.Core.Dtos.Auth;

public record EmailConfirmRequestDto(string Id, string Token)
{
    public void Deconstruct(out string id, out string token)
    {
        id = Id;
        token = Token;
    }
};
