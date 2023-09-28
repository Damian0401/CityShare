using CityShare.Backend.Application.Core.Abstractions.Emails;
using CityShare.Backend.Application.Core.Dtos.Emails;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Emails.Commands;

public record SendPendingEmailsCommand : IRequest<Result<SendPendingEmailsDto>>;

public class SendPendingEmailsCommandHandler : IRequestHandler<SendPendingEmailsCommand, Result<SendPendingEmailsDto>>
{
    private readonly IEmailRepository _emailRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<SendPendingEmailsCommandHandler> _logger;

    public SendPendingEmailsCommandHandler(
        IEmailRepository emailRepository,
        IEmailService emailService,
        ILogger<SendPendingEmailsCommandHandler> logger)
    {
        _emailRepository = emailRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<Result<SendPendingEmailsDto>> Handle(SendPendingEmailsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching for pending emails");
        var pendingEmails = await _emailRepository.GetAllWithStatusAsync(EmailStatuses.Pending, cancellationToken);

        if (!pendingEmails.Any())
        {
            _logger.LogInformation("Not found any pending emails");
            return new SendPendingEmailsDto(0, 0, 0);
        }

        _logger.LogInformation("Found {@Number} pending emails", pendingEmails.Count());
        var response = await SendPendingEmailsAsync(pendingEmails, cancellationToken);

        return response;
    }

    private async Task<SendPendingEmailsDto> SendPendingEmailsAsync(IEnumerable<Email> pendingEmails, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching email priorities");
        var priorities = await _emailRepository.GetAllPrioritiesAsync(cancellationToken);

        _logger.LogInformation("Grouping emails by priorities");
        var groupedEmails = pendingEmails
            .GroupBy(x => x.PrirorityId)
            .ToList();

        var sentEmails = 0;
        var notSentEmails = 0;
        var errorEmails = 0;

        var errorStatusId = await _emailRepository.GetStatusIdAsync(EmailStatuses.Error, cancellationToken);
        var sentStatusId = await _emailRepository.GetStatusIdAsync(EmailStatuses.Sent, cancellationToken);

        _logger.LogInformation("Trying to resend emails");
        foreach (var group in groupedEmails)
        {
            var priority = priorities.First(x => x.Id.Equals(group.Key));

            (var sent, var notSent, var error) = await SendEmailGroup(group, priority, errorStatusId, sentStatusId);

            sentEmails += sent;
            notSentEmails += notSent;
            errorEmails += error;
        }

        _logger.LogInformation("Updating emails after resending");
        await _emailRepository.UpdateEmailsAsync(pendingEmails);

        return new SendPendingEmailsDto(sentEmails, notSentEmails, errorEmails);
    }

    private async Task<(int sentEmails, int notSentEmails, int errorEmails)> SendEmailGroup(IGrouping<int, Email> group, EmailPriority priority, int errorStatusId, int sentStatusId)
    {
        int sentEmails = 0;
        int notSentEmails = 0;
        int errorEmails = 0;

        foreach (var email in group)
        {
            if (email.TryCount >= priority.RetryNumber)
            {
                _logger.LogInformation("Skipping sending email with id {@Id}", email.Id);
                email.StatusId = errorStatusId;
                errorEmails++;
                continue;
            }

            try
            {
                _logger.LogInformation("Sending email with id {@Id}", email.Id);
                await _emailService.SendAsync(email);

                email.StatusId = sentStatusId;
                email.SentDate = DateTime.UtcNow;
                sentEmails++;
            }
            catch (Exception)
            {
                _logger.LogError("Unable to send email with id {@Id}", email.Id);
                email.TryCount++;
                notSentEmails++;
            }
        }

        return (sentEmails, notSentEmails, errorEmails);
    }
}
