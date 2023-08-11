namespace CityShare.Backend.Application.Core.Models.Authentication.ConfirmEmail;

public record EmailConfirmRequestModel(string Id, string Token)
{
    public void Deconstruct(out string id, out string token)
    {
        id = Id; 
        token = Token;
    }
};
