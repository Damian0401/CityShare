namespace CityShare.Backend.Application.Core.Dtos.Emails;

public record CreateEmailDto(
    string Receiver,
    string Template,
    Dictionary<string, string> Parameters);
