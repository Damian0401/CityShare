namespace CityShare.Backend.Application.Core.Abstractions.Images;

public interface IImageService
{
    Task<Stream> BlurFacesAsync(Stream input, CancellationToken cancellationToken = default);
}
