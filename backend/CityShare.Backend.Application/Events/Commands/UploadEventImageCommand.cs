using CityShare.Backend.Application.Core.Abstractions.Blobs;
using CityShare.Backend.Application.Core.Abstractions.Events;
using CityShare.Backend.Application.Core.Abstractions.Images;
using CityShare.Backend.Application.Core.Abstractions.Queues;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Extensions;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Events.Commands;

public class UploadEventImageCommand : IRequest<Result>
{
    public IFormFile Image { get; init; } = default!;
    public Guid EventId { get; init; }
    public string UserId { get; init; } = default!;
    public bool? ShouldBeBlurred { get; init; }
}

public class UploadEventImageCommandValidator : AbstractValidator<UploadEventImageCommand>
{
    public UploadEventImageCommandValidator()
    {
        RuleFor(x => x.Image)
            .NotEmpty();

        RuleFor(x => x.EventId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}

public class UploadEventImageCommandHandler : IRequestHandler<UploadEventImageCommand, Result>
{
    private readonly IImageRepository _imageRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IBlobService _blobService;
    private readonly IQueueService _queueService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UploadEventImageCommandHandler> _logger;

    public UploadEventImageCommandHandler(
        IImageRepository imageRepository,
        IEventRepository eventRepository,
        IBlobService blobService,
        IQueueService queueService,
        UserManager<ApplicationUser> userManager,
        ILogger<UploadEventImageCommandHandler> logger)
    {
        _imageRepository = imageRepository;
        _eventRepository = eventRepository;
        _blobService = blobService;
        _queueService = queueService;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result> Handle(UploadEventImageCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Validating {@Type} request", request.GetType());
        var errors = await ValidateAsync(request, cancellationToken);

        if (errors.Any())
        {
            _logger.LogError("{@Type} contains invaid data {@Errors}", request.GetType(), errors);
            return Result.Failure(errors);
        }

        _logger.LogInformation("Creating image");
        var imageId = await CreateImageAsync(request);

        _logger.LogInformation("Uploading image to blob");
        var uri = await UploadImageAsync(request.Image, imageId, cancellationToken);

        if (request.ShouldBeBlurred ?? false)
        {
            _logger.LogInformation("Sending image to {@Name} queue", QueueNames.ImagesToBlur);
            await SendImageToQueueAsync(imageId, cancellationToken);
        }

        _logger.LogInformation("Setting uri for image with id {@Id}", imageId);
        await _imageRepository.SetImageUriAsync(imageId, uri);

        return Result.Success();
    }

    private async Task SendImageToQueueAsync(Guid imageId, CancellationToken cancellationToken)
    {
        var queueOptions = new QueueServiceOptions
        {
            CreateIfNotExists = true,
            EncodeToBase64 = true
        };

        await _queueService.SendAsync(QueueNames.ImagesToBlur, imageId, queueOptions, cancellationToken);
    }

    private async Task<Guid> CreateImageAsync(UploadEventImageCommand request)
    {
        var image = new Image
        {
            EventId = request.EventId,
            ShouldBeBlurred = request.ShouldBeBlurred ?? false,
        };

        var imageId = await _imageRepository.CreateAsync(image);
        return imageId;
    }

    private async Task<string> UploadImageAsync(IFormFile image, Guid imageId, CancellationToken cancellationToken)
    {
        var uploadOptions = new BlobServiceOptions
        {
            AnonymousRead = true,
            CreateIfNotExists = true,
            BlobName = imageId.ToString()
        };

        var uri = await _blobService.UploadFileAsync(image, ContainerNames.EventImages, uploadOptions, cancellationToken);
        return uri;
    }

    private async Task<IEnumerable<Error>> ValidateAsync(UploadEventImageCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking if of user with id {@Id} is confirmed", request.UserId);
        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(request.UserId);

        if (!isEmailConfirmed)
        {
            _logger.LogError("Email of user with id {@Id} is not confirmed", request.UserId);
            return Errors.Forbidden;
        }

        var eventExists = await _eventRepository.ExistsAsync(request.EventId, cancellationToken);

        if (!eventExists)
        {
            _logger.LogError("Event with id {@Id} does not exists", request.EventId);
            return Errors.NotFound;
        }

        var imagesNumber = await _eventRepository.GetImagesNumberAsync(request.EventId);

        if (imagesNumber >= Constants.MaxEventImagesNumber)
        {
            _logger.LogError("Reached max event images number: {@Number}", Constants.MaxEventImagesNumber);
            return Errors.MaxImagesNumber;
        }

        return Enumerable.Empty<Error>();
    }
}