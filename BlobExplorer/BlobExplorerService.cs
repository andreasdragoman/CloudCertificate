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

        public async Task UploadBlob(string localPath = "./data/", string fileName = "dummy.txt", string blobName = "dummy.txt", string fullPath = "")
        {
            Console.WriteLine("Uploading to Blob storage as blob.\n");

            //if (!Directory.Exists(localPath))
            //{
            //    Directory.CreateDirectory(localPath);
            //}

            //string localFilePath = Path.Combine(localPath, fileName);
            // Write text to the file
            //await File.WriteAllTextAsync(localFilePath, "Hello, World!");

            var container = GetBlobContainerClient();
            var blob = GetBlobClient(container, blobName);

            //await blob.UploadAsync(localFilePath, true);

            // Open the file and upload its data
            using (FileStream uploadFileStream = File.OpenRead(fullPath))
            {
                await blob.UploadAsync(uploadFileStream, true);
                uploadFileStream.Close();
            }

            Console.WriteLine("\nThe file was uploaded. We'll verify by listing the blobs next.");
        }

        public async Task<List<string>> GetBlobsNames()
        {
            BlobContainerClient container = GetBlobContainerClient();
            var blobsNames = new List<string>();

            await foreach (var blob in container.GetBlobsAsync())
            {
                Console.WriteLine($"Blob name: {blob.Name}");
                blobsNames.Add(blob.Name);
            }
            return blobsNames;
        }

        public async Task DownloadBlob(string localPath = "./data/", string fileName = "dummy.txt", string blobName = "dummy.txt")
        {
            BlobContainerClient container = GetBlobContainerClient();

            string localFilePath = Path.Combine(localPath, fileName);
            string downloadFilePath = localFilePath.Replace(".txt", "DOWNLOADED.txt");
            Console.WriteLine("\nDownloading blob to\n\t{0}\n", downloadFilePath);

            var blob = container.GetBlobClient(blobName);

            // Download the blob's contents and save it to a file
            BlobDownloadInfo download = await blob.DownloadAsync();

            using (FileStream downloadFileStream = File.OpenWrite(downloadFilePath))
            {
                await download.Content.CopyToAsync(downloadFileStream);
                downloadFileStream.Close();
            }

            Console.WriteLine("Download complete.");
        }

        public async Task DeleteBlob(string blobName = "dummy.txt")
        {
            BlobContainerClient container = GetBlobContainerClient();

            Console.WriteLine("\n\nDeleting blob container...");

            var blob = container.GetBlobClient(blobName);
            await blob.DeleteIfExistsAsync();

            Console.WriteLine("\n\nBlob container deleted.");

            //Console.WriteLine("Deleting the local source and downloaded files...");
            string localPath = "./data/";
            string localFilePath = Path.Combine(localPath, blobName);
            if(File.Exists(localFilePath))
            {
                File.Delete(localFilePath);
            }
            //Console.WriteLine("Finished cleaning up.");
        }

        public async Task DeleteAllBlobs()
        {

            var blobNames = await GetBlobsNames();
            foreach (var blob in blobNames)
            {
                await DeleteBlob(blob);
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
