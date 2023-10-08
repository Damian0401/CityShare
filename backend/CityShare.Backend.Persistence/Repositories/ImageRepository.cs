using CityShare.Backend.Application.Core.Abstractions.Images;
using CityShare.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Persistence.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly CityShareDbContext _context;
    private readonly ILogger<ImageRepository> _logger;

    public ImageRepository(
        CityShareDbContext context,
        ILogger<ImageRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Guid> CreateAsync(Image image, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Adding {@Image} to database", image);
        _context.Images.Add(image);
        await _context.SaveChangesAsync(cancellationToken);

        return image.Id;
    }

    public async Task<Image?> GetByIdAsync(Guid imageId, CancellationToken cancellationToken = default)
    {
        var image = await _context.Images
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(imageId), cancellationToken);

        return image;
    }

    public async Task SetUriAsync(Guid imageId, string imageUri, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating {@Name} for image with id {@Id} in database", nameof(Image.Uri), imageId);
        await _context.Images
            .Where(x => x.Id.Equals(imageId))
            .ExecuteUpdateAsync(x => x.SetProperty(i => i.Uri, imageUri), cancellationToken);
    }

    public async Task SetIsBlurredAsync(Guid imageId, bool isBlurred, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Setting {@Name} as true for image with id {@Id} in database", nameof(Image.IsBlurred), imageId);
        await _context.Images
            .Where(x => x.Id.Equals(imageId))
            .ExecuteUpdateAsync(x => x.SetProperty(i => i.IsBlurred, isBlurred), cancellationToken);
    }
}
