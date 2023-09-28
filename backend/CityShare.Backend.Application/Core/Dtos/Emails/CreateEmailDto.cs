namespace CityShare.Backend.Application.Core.Dtos.Emails;

public record CreateEmailDto(
    string Receiver,
    string Template,
    string Priority,
    Dictionary<string, string> Parameters);
