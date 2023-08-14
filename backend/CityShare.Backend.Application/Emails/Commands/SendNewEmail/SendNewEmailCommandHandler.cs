using CityShare.Backend.Application.Core.Abstractions.Emails;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Emails.Commands.SendNewEmail;

public class SendNewEmailCommandHandler : IRequestHandler<SendNewEmailCommand, Result>
{
    private readonly IEmailRepository _emailRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<SendNewEmailCommandHandler> _logger;

    public SendNewEmailCommandHandler(
        IEmailRepository emailRepository,
        IEmailService emailService,
        ILogger<SendNewEmailCommandHandler> logger)
    {
        _emailRepository = emailRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<Result> Handle(SendNewEmailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching for email with id {@Id}", request.EmailId);
        var email = await _emailRepository.GetByIdAsync(request.EmailId, cancellationToken);

        if (email is null)
        {
            _logger.LogError("Email with id {@Id} not found", request.EmailId);
            return Result.Failure(Errors.NotFound);
        }

        if (!email.Status.Equals(EmailStatuses.New))
        {
            _logger.LogError("Email {@Email} has wrong status", email);
            return Result.Failure(Errors.ForbiddenState);
        }

        _logger.LogInformation("Changing email status to {@Status}", EmailStatuses.Pending);
        email.Status = EmailStatuses.Pending;

        try
        {
            _logger.LogInformation("Trying to send email {@Email}", email);
            await _emailService.SendAsync(email);
        }
        catch
        {
            _logger.LogInformation("Unable to send email {@Email}, updating data", email);
            email.TryCount++;
            await _emailRepository.UpdateAsync(email);
            return Result.Failure(Errors.OperationFailed);
        }

        _logger.LogInformation("Update email {@Email} data after sending", email);
        email.Status = EmailStatuses.Send;
        email.SendDate = DateTime.UtcNow;
        await _emailRepository.UpdateAsync(email);
        return Result.Success();
    }
}
