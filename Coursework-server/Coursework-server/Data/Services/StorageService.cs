using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Coursework_server.Data.Services
{
    public class StorageService
    {
        private readonly string _storageConnectionString;

        public StorageService(IConfiguration configuration)
        {
            _storageConnectionString = configuration.GetConnectionString("StorageConnectionString");
        }

        public async Task<string?> UploadFileAsync(IFormFile file)
        {
            if (file.Length == 0)
            {
                return null;
            }
            
            var container = new BlobContainerClient(_storageConnectionString, "image-container");
            var createResponse = await container.CreateIfNotExistsAsync();

            if (createResponse != null && createResponse.GetRawResponse().Status == 201)
            {
                await container.SetAccessPolicyAsync(PublicAccessType.Blob);
            }

            var newFileName = Guid.NewGuid().ToString();
            var blob = container.GetBlobClient(newFileName);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

            await using (var fileStream = file.OpenReadStream())
            {
                await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = file.ContentType });
            }

            return blob.Uri.ToString();
        }
    }
}
