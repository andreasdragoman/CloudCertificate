using Azure.Storage.Blobs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlobExplorer
{
    public interface IBlobExplorerService
    {
        Task DeleteBlob(string path, string blobName = "dummy.txt");
        Task DeleteAllBlobs(string localPath);
        Task DownloadBlob(string localPath = "./data/", string fileName = "dummy.txt", string blobName = "dummy.txt");
        Task<List<string>> GetBlobsNames();
        Task UploadBlob(string localPath = "./data/", string fileName = "dummy.txt", string blobName = "dummy.txt", string fullPath = "");
    }
}