using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CityShare.Backend.Application.Core.Abstractions.Blob;
using Microsoft.AspNetCore.Http;

namespace CityShare.Backend.Infrastructure.Blob;

public class StorageBlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;

    public StorageBlobService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadFileAsync(IFormFile file,
        string containerName,
        BlobServiceOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var container = await GetContainerAsync(containerName, options, cancellationToken);

        var name = options?.BlobName ?? Guid.NewGuid().ToString();

        var client = container.GetBlobClient(name);

        using var stream = file.OpenReadStream();

        var overwrite = options?.Overwrite ?? false;
        await client.UploadAsync(stream, overwrite, cancellationToken);

        return client.Uri.AbsoluteUri;
    }

    private async Task<BlobContainerClient> GetContainerAsync(string containerName, BlobServiceOptions? options, CancellationToken cancellationToken)
    {
        var container = _blobServiceClient.GetBlobContainerClient(containerName);

        var createIfNotExists = options?.CreateIfNotExists ?? false;
        var anonymousRead = (options?.AnonymousRead ?? false) && !container.Exists();

        if (!createIfNotExists)
        {
            return container;
        }

        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        if (anonymousRead)
        {
            await container.SetAccessPolicyAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);
        }

        return container;
    }
}
