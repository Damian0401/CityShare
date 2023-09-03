namespace CityShare.Backend.Application.Core.Dtos.Authentication.ConfirmEmail;

public record EmailConfirmRequestDto(string Id, string Token)
{
    public void Deconstruct(out string id, out string token)
    {
        id = Id; 
        token = Token;
    }
};
