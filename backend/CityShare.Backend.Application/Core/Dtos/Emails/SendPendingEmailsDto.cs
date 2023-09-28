namespace CityShare.Backend.Application.Core.Dtos.Emails;

public record SendPendingEmailsDto(
    int SentEmails,
    int NotSentEmails,
    int ErrorEmails);
