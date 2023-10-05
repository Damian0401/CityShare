using CityShare.Backend.Application.Core.Abstractions.Emails;
using CityShare.Backend.Application.Core.Abstractions.Utils;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Emails.Commands;

public record SendPendingEmailCommand(Guid EmailId) : IRequest<Result>;

public class SendPendingEmailCommandValidator : AbstractValidator<SendPendingEmailCommand>
{
    public SendPendingEmailCommandValidator()
    {
        RuleFor(x => x.EmailId)
            .NotEmpty();
    }
}

public class SendPendingEmailCommandHandler : IRequestHandler<SendPendingEmailCommand, Result>
{
    private readonly IEmailRepository _emailRepository;
    private readonly IEmailService _emailService;
    private readonly IClock _clock;
    private readonly ILogger<SendPendingEmailCommandHandler> _logger;

    public SendPendingEmailCommandHandler(
        IEmailRepository emailRepository,
        IEmailService emailService,
        IClock clock,
        ILogger<SendPendingEmailCommandHandler> logger)
    {
        _emailRepository = emailRepository;
        _emailService = emailService;
        _clock = clock;
        _logger = logger;
    }

    public async Task<Result> Handle(SendPendingEmailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching for email with id {@Id}", request.EmailId);
        var email = await _emailRepository.GetByIdAsync(request.EmailId, cancellationToken);

        if (email is null)
        {
            _logger.LogError("Email with id {@Id} not found", request.EmailId);
            return Result.Failure(Errors.NotFound);
        }

        var pendingStatusId = await _emailRepository.GetStatusIdAsync(EmailStatuses.Pending, cancellationToken);

        if (!email.StatusId.Equals(pendingStatusId))
        {
            _logger.LogError("Email with id {@Id} has wrong StatusId {@StatusId}, expected {@CorrectStatusId}", email.Id, email.StatusId, pendingStatusId);
            return Result.Failure(Errors.ForbiddenState);
        }

        _logger.LogInformation("Trying to send email with id {@Id}", email.Id);
        await _emailService.SendAsync(email);

        _logger.LogInformation("Updating email with id {@Id} after sending", email.Id);
        var sentStatusId = await _emailRepository.GetStatusIdAsync(EmailStatuses.Sent, cancellationToken);
        email.StatusId = sentStatusId;
        email.SentDate = _clock.Now;
        await _emailRepository.UpdateAsync(email);

        return Result.Success();
    }
}
