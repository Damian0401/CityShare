namespace CityShare.Backend.Application.Core.Models.Emails.Create;

public record CreateEmailModel(
    string Receiver,
    string Template,
    string Priority,
    Dictionary<string, string> Parameters);
