using CityShare.Backend.Application.Core.Abstractions.Blobs;
using CityShare.Backend.Application.Core.Abstractions.Images;
using CityShare.Backend.Domain.Constants;
using CityShare.Backend.Domain.Entities;
using CityShare.Backend.Domain.Shared;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Application.Images.Commands;

public record BlurFacesCommand(Guid ImageId) : IRequest<Result>;

public class BlurFacesCommandValidator : AbstractValidator<BlurFacesCommand>
{
    public BlurFacesCommandValidator()
    {
        RuleFor(x => x.ImageId)
            .NotEmpty();
    }
}

public class BlurFacesCommandHandler : IRequestHandler<BlurFacesCommand, Result>
{
    private readonly IImageRepository _imageRepository;
    private readonly IBlobService _blobService;
    private readonly IImageService _imageService;
    private readonly ILogger<BlurFacesCommandHandler> _logger;

    public BlurFacesCommandHandler(
        IImageRepository imageRepository,
        IBlobService blobService,
        IImageService imageService,
        ILogger<BlurFacesCommandHandler> logger)
    {
        _imageRepository = imageRepository;
        _blobService = blobService;
        _imageService = imageService;
        _logger = logger;
    }

    public async Task<Result> Handle(BlurFacesCommand request, CancellationToken cancellationToken)
    {
        var image = await GetImageAsync(request, cancellationToken);

        if (image is null)
        {
            _logger.LogError("Image with id {@Id} not found", request.ImageId);
            return Result.Failure(Errors.NotFound);
        }

        if (!image.ShouldBeBlurred)
        {
            _logger.LogInformation("Image with id {@Id} already blurred", request.ImageId);
            return Result.Failure(Errors.ForbiddenState);
        }

        using var stream = await GetImageBlobAsync(image.Uri, cancellationToken);

        if (stream is null)
        {
            _logger.LogError("Image blob with id {@Id} not found in {@Container}",
                request.ImageId, ContainerNames.EventImages);

            return Result.Failure(Errors.NotFound);
        }

        using var blurredStream = await BlurImageAsync(request, stream, cancellationToken);

        await UpdateImageAsync(image, blurredStream, cancellationToken);

        return Result.Success();
    }

    private async Task<Image?> GetImageAsync(BlurFacesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting image with id {@Id} using {@Type}",
                    request.ImageId, _imageRepository.GetType());

        var image = await _imageRepository.GetByIdAsync(
            request.ImageId,
            cancellationToken);

        return image;
    }

    private async Task<Stream?> GetImageBlobAsync(string uri, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting image blob from {@Uri} using {@Type}",
                    uri, _blobService.GetType());

        var fileName = uri.Split('/').Last();

        var stream = await _blobService.ReadFileAsync(
                    fileName,
                    ContainerNames.EventImages,
                    cancellationToken);

        return stream;
    }

    private async Task<Stream> BlurImageAsync(BlurFacesCommand request, Stream stream, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Blurring faces from image with id {@Id} using {@Type}",
                    request.ImageId, _imageService.GetType());

        var blurredStream = await _imageService.BlurFacesAsync(
                    stream,
                    cancellationToken);

        return blurredStream;
    }

    private async Task UpdateImageAsync(Image image, Stream blurredStream, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Overriding image with id {@Id} using {@Type} after blurring",
                    image.Id, _blobService.GetType());

        var options = new BlobServiceUploadOptions
        {
            Overwrite = true,
        };

        var fileName = image.Uri.Split('/').Last();

        await _blobService.UploadFileAsync(
            blurredStream,
            fileName,
            ContainerNames.EventImages,
            options,
            cancellationToken);

        _logger.LogInformation("Setting image with id {@Id} as blurred using {@Type} after overriding",
            image.Id, _imageRepository.GetType());

        await _imageRepository.SetIsBlurredAsync(image.Id, true, cancellationToken);
    }
}
