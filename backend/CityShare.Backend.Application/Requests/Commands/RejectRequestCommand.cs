using CityShare.Backend.Application.Core.Abstractions.Emails;
using CityShare.Backend.Application.Core.Abstractions.Images;
using CityShare.Backend.Application.Core.Abstractions.Queues;
using CityShare.Backend.Application.Core.Abstractions.Requests;
using CityShare.Backend.Application.Core.Dtos.Emails;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Exceptions;
using CityShare.Backend.Domain.Settings;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CityShare.Backend.Application.Requests.Commands;

public record RejectRequestCommand(Guid RequestId) : IRequest<Result>;

public class RejectRequestCommandValidator : AbstractValidator<RejectRequestCommand>
{
    public RejectRequestCommandValidator()
    {
        RuleFor(x => x.RequestId).NotEmpty();
    }
}

public class RejectRequestCommandHandler : IRequestHandler<RejectRequestCommand, Result>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IEmailRepository _emailRepository;
    private readonly IQueueService _queueService;
    private readonly IImageRepository _imageRepository;
    private readonly CommonSettings _commonSettings;
    private readonly ILogger<RejectRequestCommandHandler> _logger;

    public RejectRequestCommandHandler(
        IRequestRepository requestRepository, 
        IEmailRepository emailRepository,
        IQueueService queueService, 
        IImageRepository imageRepository,
        IOptions<CommonSettings> options,
        ILogger<RejectRequestCommandHandler> logger)
    {
        _requestRepository = requestRepository;
        _emailRepository = emailRepository;
        _queueService = queueService;
        _imageRepository = imageRepository;
        _commonSettings = options.Value;
        _logger = logger;
    }

    public async Task<Result> Handle(RejectRequestCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting request with id {@Id} from {@Type}", request.RequestId, _requestRepository.GetType());
        var userRequest = await _requestRepository.GetByIdWithDetailsAsync(request.RequestId, cancellationToken);

        _logger.LogInformation("Validating request {@Request}", request);
        var errors = Validate(userRequest);

        if (errors.Any())
        {
            _logger.LogError("Request {@Request} is not valid", request);
            return Result.Failure(errors);
        }

        _logger.LogInformation("Getting status id for {@Status}", RequestStatuses.Rejected);
        var statusId = await _requestRepository.GetStatusIdAsync(RequestStatuses.Rejected, cancellationToken);

        _logger.LogInformation("Updating status of request with id {@Id} to {@Status}", request.RequestId, RequestStatuses.Accepted);
        await _requestRepository.UpdateStatusAsync(request.RequestId, statusId, cancellationToken);

        _logger.LogInformation("Sending confirmation email for request with id {@Id}", request.RequestId);
        await SendConfirmationEmailAsync(userRequest!, cancellationToken);

        return Result.Success();
    }

    private async Task SendConfirmationEmailAsync(Request request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating CreateEmailDto");
        var dto = new CreateEmailDto(
            request.Author.Email ?? throw new InvalidStateException(),
            EmailTemplates.RequestRejected,
            new Dictionary<string, string>
            {
                { EmailPlaceholders.UserName, request.Author?.UserName ?? string.Empty },
                { EmailPlaceholders.RequestType, request.Type.Name },
                { EmailPlaceholders.EventTitle, request.Image?.Event.Title ?? string.Empty },
                { EmailPlaceholders.ClientUrl, _commonSettings.ClientUrl },
                { EmailPlaceholders.EventId, request.Image?.Event.Id.ToString() ?? string.Empty },
            });

        _logger.LogInformation("Creating email from dto {@Dto}", dto);
        var emailId = await _emailRepository.CreateAsync(dto, cancellationToken);

        _logger.LogInformation("Sending emailId {@Id} to queue {@Queue}", emailId, QueueNames.EmailsToSend);
        var options = new QueueServiceSendOptions
        {
            CreateIfNotExists = true,
            EncodeToBase64 = true,
        };
        await _queueService.SendAsync(QueueNames.EmailsToSend, emailId, options, cancellationToken);
    }

    private IEnumerable<Error> Validate(Request? request)
    {
        if (request is null)
        {
            _logger.LogError("Request not found");
            return Errors.NotFound;
        }

        var errors = new List<Error>();

        if (request.Status.Name != RequestStatuses.Pending)
        {
            _logger.LogInformation("Request with id {@Id} is not pending", request.Id);
            errors.AddRange(Errors.RequestNotPending);
        }

        if (request.ImageId is null)
        {
            _logger.LogError("Request with id {@Id} does not have an image", request.Id);
            errors.AddRange(Errors.RequestHasNoImage);
        }

        return errors;
    }
}