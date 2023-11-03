using CityShare.Backend.Application.Core.Abstractions.Blobs;
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

public record AcceptRequestCommand(Guid RequestId) : IRequest<Result>;

public class AcceptRequestCommandValidator : AbstractValidator<AcceptRequestCommand>
{
    public AcceptRequestCommandValidator()
    {
        RuleFor(x => x.RequestId).NotEmpty();
    }
}

public class AcceptRequestCommandHandler : IRequestHandler<AcceptRequestCommand, Result>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IEmailRepository _emailRepository;
    private readonly IQueueService _queueService;
    private readonly IImageRepository _imageRepository;
    private readonly IBlobService _blobService;
    private readonly CommonSettings _commonSettings;
    private readonly ILogger<AcceptRequestCommandHandler> _logger;

    public AcceptRequestCommandHandler(IRequestRepository requestRepository, 
        IEmailRepository emailRepository,
        IQueueService queueService, 
        IImageRepository imageRepository,
        IBlobService blobService,
        IOptions<CommonSettings> options,
        ILogger<AcceptRequestCommandHandler> logger)
    {
        _requestRepository = requestRepository;
        _emailRepository = emailRepository;
        _queueService = queueService;
        _imageRepository = imageRepository;
        _blobService = blobService;
        _commonSettings = options.Value;
        _logger = logger;
    }

    public async Task<Result> Handle(AcceptRequestCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting request with id {@Id} from {@Type}", request.RequestId, _requestRepository.GetType());
        var userRequest = await _requestRepository.GetByIdWithDetailsAsync(request.RequestId, cancellationToken);

        _logger.LogInformation("Validating request {@Request}", request);
        var errors = Validate(request.RequestId, userRequest);

        if (errors.Any())
        {
            _logger.LogError("Request {@Request} is not valid", request);
            return Result.Failure(errors);
        }

        _logger.LogInformation("Getting status id for {@Status}", RequestStatuses.Accepted);
        var statusId = await _requestRepository.GetStatusIdAsync(RequestStatuses.Accepted, cancellationToken);

        _logger.LogInformation("Executing request with id {@Id}", request.RequestId);
        await ExecuteRequestAsync(userRequest!.Type.Name, userRequest.ImageId!.Value, cancellationToken);
        
        _logger.LogInformation("Updating status of request with id {@Id} to {@Status}", request.RequestId, RequestStatuses.Accepted);
        await _requestRepository.UpdateStatusAsync(request.RequestId, statusId, cancellationToken);

        _logger.LogInformation("Sending confirmation email for request with id {@Id}", request.RequestId);
        await SendConfirmationEmailAsync(userRequest, cancellationToken);

        return Result.Success();
    }

    private IEnumerable<Error> Validate(Guid requestId, Request? request)
    {
        if (request is null)
        {
            _logger.LogError("Request with id {@Id} not found", requestId);
            return Errors.NotFound;
        }

        if (!request.Status.Name.Equals(RequestStatuses.Pending))
        {
            _logger.LogError("Request with id {@Id} is not in pending state", requestId);
            return Errors.RequestNotPending;
        }

        if (request.ImageId is null)
        {
            _logger.LogError("Request with id {@Id} does not have an image", requestId);
            return Errors.RequestHasNoImage;
        }

        return Enumerable.Empty<Error>();
    }

    private async Task SendConfirmationEmailAsync(Request request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating CreateEmailDto");
        var dto = new CreateEmailDto(
            request.Author.Email ?? throw new InvalidStateException(),
            EmailTemplates.RequestAccepted,
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

    private async Task ExecuteRequestAsync(string type, Guid imageId, CancellationToken cancellationToken)
    {
        switch (type)
        {
            case RequestTypes.Blur:
                await ExecuteBlurRequestAsync(imageId, cancellationToken);
                break;
            case RequestTypes.Delete:
                await ExecuteDeleteRequestAsync(imageId, cancellationToken);
                break;
            default:
                throw new InvalidStateException();
        }
    }

    private async Task ExecuteBlurRequestAsync(Guid imageId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Setting image with id {@Id} to be blurred", imageId);
        await _imageRepository.SetShouldBeBlurredAsync(imageId, true, cancellationToken);

        var queueOptions = new QueueServiceSendOptions
        {
            CreateIfNotExists = true,
            EncodeToBase64 = true
        };

        _logger.LogInformation("Sending imageId {@Id} to queue {@Queue}", imageId, QueueNames.ImagesToBlur);
        await _queueService.SendAsync(QueueNames.ImagesToBlur, imageId, queueOptions, cancellationToken);
    }

    private async Task ExecuteDeleteRequestAsync(Guid imageId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting image with id {@Id} from blob storage", imageId);
        await _blobService.DeleteFileAsync(imageId.ToString(), ContainerNames.EventImages, cancellationToken);

        _logger.LogInformation("Deleting image with id {@Id}", imageId);
        await _imageRepository.DeleteAsync(imageId, cancellationToken);
    }
}