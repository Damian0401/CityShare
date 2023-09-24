using CityShare.Backend.Application.Core.Abstractions.Emails;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Emails.Commands;

public record SendNewEmailCommand(Guid EmailId) : IRequest<Result>;

public class SendNewEmailCommandValidator : AbstractValidator<SendNewEmailCommand>
{
    public SendNewEmailCommandValidator()
    {
        RuleFor(x => x.EmailId)
            .NotEmpty();
    }
}

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

        var newStatusId = await _emailRepository.GetStatusIdAsync(EmailStatuses.New, cancellationToken);

        if (!email.StatusId.Equals(newStatusId))
        {
            _logger.LogError("Email with id {@Id} has wrong StatusId {@StatusId}, expected {@CorrectStatusId}", email.Id, email.StatusId, newStatusId);
            return Result.Failure(Errors.ForbiddenState);
        }

        try
        {
            _logger.LogInformation("Trying to send email with id {@Id}", email.Id);
            await _emailService.SendAsync(email);
        }
        catch
        {
            _logger.LogInformation("Unable to send email with id {@Id}, updating Status and TryCount", email.Id);
            var pendingStatusId = await _emailRepository.GetStatusIdAsync(EmailStatuses.Pending, cancellationToken);
            email.StatusId = pendingStatusId;
            email.TryCount++;
            await _emailRepository.UpdateAsync(email);
            return Result.Failure(Errors.OperationFailed);
        }

        _logger.LogInformation("Updating email with id {@Id} after sending", email.Id);
        var sendStatusId = await _emailRepository.GetStatusIdAsync(EmailStatuses.Sent, cancellationToken);
        email.StatusId = sendStatusId;
        email.SentDate = DateTime.UtcNow;
        await _emailRepository.UpdateAsync(email);
        return Result.Success();
    }
}
