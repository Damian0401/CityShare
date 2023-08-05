using CityShare.Backend.Application.Core.Abstractions.Emails;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace CityShare.Backend.Infrastructure.Emails;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private EmailSettings _emailSettings;

    public EmailService(ILogger<EmailService> logger, IOptions<EmailSettings> options)
    {
        _logger = logger;
        _emailSettings = options.Value;
    }

    public async Task SendAsync(Email email)
    {
        _logger.LogInformation("Creating MailMessage from {@Email}", email);
        using var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailSettings.Address),
            IsBodyHtml = true,
            Subject = email.Subject,
            Body = email.Body,
        };
        mailMessage.To.Add(new MailAddress(email.To));

        _logger.LogInformation("Creating smtpClient from {@Settings}", _emailSettings);
        using var smtpClient = new SmtpClient
        {
            Port = _emailSettings.Port,
            EnableSsl = _emailSettings.EnableSSL,
            Host = _emailSettings.Host,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Timeout = _emailSettings.TimeoutMiliseconds,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_emailSettings.Address, _emailSettings.Password)
        };

        _logger.LogInformation("Sending email {@Email}", mailMessage);
        await smtpClient.SendMailAsync(mailMessage);
    }
}
