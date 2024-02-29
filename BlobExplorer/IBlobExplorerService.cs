using Azure.Storage.Blobs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlobExplorer
{
    public interface IBlobExplorerService
    {
        Task DeleteBlob(string localPath, string blobName);
        Task DeleteAllBlobs(string localPath);
        Task DownloadBlob(string localPath, string fileName, string blobName);
        Task<List<string>> GetBlobsNames();
        Task UploadBlob(string fullPath, string fileName, string blobName);
    }
}