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

        using var stream = await GetImageBlobAsync(request, cancellationToken);

        if (stream is null)
        {
            _logger.LogError("Image blob with id {@Id} not found in {@Container}",
                request.ImageId, ContainerNames.EventImages);

            return Result.Failure(Errors.NotFound);
        }

        using var blurredStream = await BlurImageAsync(request, stream, cancellationToken);

        await UpdateImageAsync(request, blurredStream, cancellationToken);

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

    private async Task<Stream?> GetImageBlobAsync(BlurFacesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting image blob with id {@Id} in {@Container} using {@Type}",
                    request.ImageId, ContainerNames.EventImages, _blobService.GetType());

        var stream = await _blobService.ReadFileAsync(
                    request.ImageId.ToString(),
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

    private async Task UpdateImageAsync(BlurFacesCommand request, Stream blurredStream, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Overriding image with id {@Id} using {@Type} after blurring",
                    request.ImageId, _blobService.GetType());

        var options = new BlobServiceUploadOptions
        {
            Overwrite = true,
        };

        await _blobService.UploadFileAsync(
            blurredStream,
            request.ImageId.ToString(),
            ContainerNames.EventImages,
            options,
            cancellationToken);

        _logger.LogInformation("Setting image with id {@Id} as blurred using {@Type} after overriding",
            request.ImageId, _imageRepository.GetType());

        await _imageRepository.SetIsBlurredAsync(request.ImageId, true, cancellationToken);
    }
}
