using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Coursework.Core.Commands;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Coursework.Core.Handlers;

public class UploadFileHandler : IRequestHandler<UploadFileCommand, string>
{
    private readonly string _storageConnectionString;

    public UploadFileHandler(IConfiguration configuration)
    {
        _storageConnectionString = configuration.GetConnectionString("StorageConnectionString");
    }

    public async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var file = request.File;
        
        if (file.Length == 0)
        {
            return null;
        }
            
        var container = new BlobContainerClient(_storageConnectionString, "image-container");
        var createResponse = await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        if (createResponse != null && createResponse.GetRawResponse().Status == 201)
        {
            await container.SetAccessPolicyAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);
        }

        var newFileName = Guid.NewGuid().ToString();
        var blob = container.GetBlobClient(newFileName);
        await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);

        await using (var fileStream = file.OpenReadStream())
        {
            await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = file.ContentType }, cancellationToken: cancellationToken);
        }

        return blob.Uri.ToString();
    }
}