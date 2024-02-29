using Microsoft.Extensions.Options;
using Azure.Identity;
using Azure.Storage.Blobs;
using System;
using System.IO;
using Azure.Storage.Blobs.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BlobExplorer
{
    public class BlobExplorerService : IBlobExplorerService
    {
        private readonly BlobExplorerSettings _settings;

        public BlobExplorerService(IOptions<BlobExplorerSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task UploadBlob(string localPath, string fileName, string blobName)
        {
            var container = GetBlobContainerClient();
            var blob = GetBlobClient(container, blobName);

            // Open the file and upload its data
            using (FileStream uploadFileStream = File.OpenRead(localPath))
            {
                await blob.UploadAsync(uploadFileStream, true);
                uploadFileStream.Close();
            }
        }

        public async Task<List<string>> GetBlobsNames()
        {
            BlobContainerClient container = GetBlobContainerClient();
            var blobsNames = new List<string>();

            await foreach (var blob in container.GetBlobsAsync())
            {
                blobsNames.Add(blob.Name);
            }
            return blobsNames;
        }

        public async Task DownloadBlob(string localPath, string fileName, string blobName)
        {
            BlobContainerClient container = GetBlobContainerClient();

            string localFilePath = Path.Combine(localPath, fileName);
            string downloadFilePath = localFilePath.Replace(".txt", "DOWNLOADED.txt");

            var blob = container.GetBlobClient(blobName);

            // Download the blob's contents and save it to a file
            BlobDownloadInfo download = await blob.DownloadAsync();

            using (FileStream downloadFileStream = File.OpenWrite(downloadFilePath))
            {
                await download.Content.CopyToAsync(downloadFileStream);
                downloadFileStream.Close();
            }
        }

        public async Task DeleteBlob(string localPath, string blobName)
        {
            BlobContainerClient container = GetBlobContainerClient();

            var blob = container.GetBlobClient(blobName);
            await blob.DeleteIfExistsAsync();

            string localFilePath = Path.Combine(localPath, blobName);
            if(File.Exists(localFilePath))
            {
                File.Delete(localFilePath);
            }
        }

        public async Task DeleteAllBlobs(string localPath)
        {
            var blobNames = await GetBlobsNames();
            foreach (var blob in blobNames)
            {
                await DeleteBlob(localPath, blob);
            }
        }

        public BlobServiceClient GetBlobServiceClient()
        {
            return new(_settings.ConnectionString);
        }

        public BlobContainerClient GetBlobContainerClient()
        {
            BlobServiceClient blobServiceClient = GetBlobServiceClient();
            return blobServiceClient.GetBlobContainerClient(_settings.ContainerName);
        }

        public BlobClient GetBlobClient(BlobContainerClient blobContainerClient, string fileName)
        {
            return blobContainerClient.GetBlobClient(Path.GetFileName(fileName));
        }
    }
}
