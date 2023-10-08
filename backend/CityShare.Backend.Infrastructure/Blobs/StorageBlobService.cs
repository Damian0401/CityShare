using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CityShare.Backend.Application.Core.Abstractions.Blobs;

namespace CityShare.Backend.Infrastructure.Blobs;

public class StorageBlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;

    public StorageBlobService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<Stream?> ReadFileAsync(string fileName, string containerName, CancellationToken cancellationToken = default)
    {
        var container = await GetContainerAsync(containerName, cancellationToken: cancellationToken);

        var client = container.GetBlobClient(fileName);

        if (!client.Exists(cancellationToken))
        {
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
        var container = await GetContainerAsync(containerName, options, cancellationToken);

        var client = container.GetBlobClient(fileName);

        var overwrite = options?.Overwrite ?? false;
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

        await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        if (anonymousRead)
        {
            await container.SetAccessPolicyAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);
        }

        return container;
    }
}
