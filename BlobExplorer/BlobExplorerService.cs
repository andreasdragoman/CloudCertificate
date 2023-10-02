using Microsoft.Extensions.Options;
using Azure.Identity;
using Azure.Storage.Blobs;
using System;
using System.IO;

namespace BlobExplorer
{
    public class BlobExplorerService
    {
        private readonly BlobExplorerSettings _settings;

        public BlobExplorerService(IOptions<BlobExplorerSettings> settings)
        {
            _settings = settings.Value;
        }

        public async void UploadBlob(string filePath = "./dummy.txt", string blobName = "./dummy.txt")
        {
            var client = GetBlobServiceClient();
            //Console.WriteLine($"Account URI: {client.Uri}");
            var container = GetBlobContainerClient(client);
            //var containerProps = await container.GetPropertiesAsync();
            //Console.WriteLine($"Container URI: {container.Uri}");
            //Console.WriteLine($"Container access level: {containerProps.Value.PublicAccess}");
            //Console.WriteLine($"Container last modified: {containerProps.Value.LastModified}");

            var blob = GetBlobClient(container, blobName);

            await blob.UploadAsync(filePath, true);
            
        }

        public void ListBlobs(BlobContainerClient container)
        {
            var blobs = container.GetBlobs();
            foreach (var blob in blobs)
            {
                Console.WriteLine($"Blob name: {blob.Name}");
            }
        }

        public async void DownloadBlob(BlobContainerClient container, string fileName)
        {
            var blob = container.GetBlobClient(fileName);
            var blobResponse = await blob.DownloadToAsync("./dummy2.txt");
            var blobContent = blobResponse.Content.ToObjectFromJson<string>();
        }

        public async void DeleteBlob(BlobContainerClient container, string fileName)
        {
            var blob = container.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync();
        }

        public BlobServiceClient GetBlobServiceClient()
        {
            return new(new Uri($"https://{_settings.AccountName}.blob.core.windows.net"), new DefaultAzureCredential());
        }

        public BlobContainerClient GetBlobContainerClient(BlobServiceClient blobServiceClient)
        {
            return blobServiceClient.GetBlobContainerClient(_settings.ContainerName);
        }

        public BlobClient GetBlobClient(BlobContainerClient blobContainerClient, string fileName)
        {
            return blobContainerClient.GetBlobClient(Path.GetFileName(fileName));
        }
    }
}
