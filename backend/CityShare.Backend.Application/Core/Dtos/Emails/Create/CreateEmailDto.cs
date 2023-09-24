namespace CityShare.Backend.Application.Core.Dtos.Emails.Create;

public record CreateEmailDto(
    string Receiver,
    string Template,
    string Priority,
    Dictionary<string, string> Parameters);
