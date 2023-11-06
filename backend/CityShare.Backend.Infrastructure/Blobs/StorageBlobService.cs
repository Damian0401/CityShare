using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CityShare.Backend.Application.Core.Abstractions.Blobs;
using Microsoft.Extensions.Logging;

namespace CityShare.Backend.Infrastructure.Blobs;

public class StorageBlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ILogger<StorageBlobService> _logger;

    public StorageBlobService(BlobServiceClient blobServiceClient, ILogger<StorageBlobService> logger)
    {
        _blobServiceClient = blobServiceClient;
        _logger = logger;
    }

    public async Task DeleteFileAsync(string fileName, string containerName, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting file {@FileName} from container {@ContainerName}", fileName, containerName);
        var container = _blobServiceClient.GetBlobContainerClient(containerName);

        var client = container.GetBlobClient(fileName);

        await client.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);
    }

    public async Task<Stream?> ReadFileAsync(string fileName, string containerName, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Reading file {@FileName} from container {@ContainerName}", fileName, containerName);
        var container = await GetContainerAsync(containerName, cancellationToken: cancellationToken);

        var client = container.GetBlobClient(fileName);

        if (!client.Exists(cancellationToken))
        {
            _logger.LogWarning("File {@FileName} does not exist in container {@ContainerName}", fileName, containerName);
            return null;
        }

        var stream = client.OpenRead();

        return stream;
    }

    public async Task<string> UploadFileAsync(
        Stream stream,
        string fileName,
        string containerName,
        BlobServiceUploadOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Uploading file {@FileName} to container {@ContainerName}", fileName, containerName);
        var container = await GetContainerAsync(containerName, options, cancellationToken);

        var client = container.GetBlobClient(fileName);

        var overwrite = options?.Overwrite ?? false;
        _logger.LogInformation("Overwrite is set to {@Overwrite}", overwrite);

        _logger.LogInformation("Uploading file {@FileName} to container {@ContainerName}", fileName, containerName);
        await client.UploadAsync(stream, overwrite, cancellationToken);

        return client.Uri.AbsoluteUri;
    }

    private async Task<BlobContainerClient> GetContainerAsync(string containerName, BlobServiceUploadOptions? options = null, CancellationToken cancellationToken = default)
    {
        var container = _blobServiceClient.GetBlobContainerClient(containerName);

        var createIfNotExists = options?.CreateIfNotExists ?? false;
        var anonymousRead = (options?.AnonymousRead ?? false) && !container.Exists();

        if (!createIfNotExists)
        {
            return container;
        }

        _logger.LogInformation("Creating container {@ContainerName} if not exists", containerName);
        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        if (anonymousRead)
        {
            _logger.LogInformation("Setting container {@ContainerName} access policy to {@AccessPolicy}", containerName, PublicAccessType.Blob);
            await container.SetAccessPolicyAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);
        }

        return container;
    }
}
