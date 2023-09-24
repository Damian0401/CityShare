namespace CityShare.Backend.Application.Core.Dtos.Emails.Send;

public record SendPendingEmailsDto(
    int SentEmails,
    int NotSentEmails,
    int ErrorEmails);
