using Azure.Storage.Blobs;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class BlobStorageService(IConfiguration configuration) : IBlobStorageService
{
    private readonly string connectionString = 
        configuration.GetConnectionString("BlobStorage")!;

    public async Task<string> UploadToBlobAsync(Stream data, string filename, string containerName)
    {
        var blobServiceClient = new BlobServiceClient(connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        var blobClient = containerClient.GetBlobClient(filename);

        await blobClient.UploadAsync(data);

        return blobClient.Uri.ToString();
    }
}
