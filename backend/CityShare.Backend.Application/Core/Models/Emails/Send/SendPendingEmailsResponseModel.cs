namespace CityShare.Backend.Application.Core.Models.Emails.Send;

public record SendPendingEmailsResponseModel(
    int SentEmails,
    int NotSentEmails,
    int ErrorEmails);
