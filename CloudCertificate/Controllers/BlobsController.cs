using BlobExplorer;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace CloudCertificate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlobsController : ControllerBase
    {
        private readonly IBlobExplorerService _blobExplorerService;
        public BlobsController(IBlobExplorerService blobExplorerService)
        {
            _blobExplorerService = blobExplorerService;
        }

        [HttpGet]
        public async Task<List<string>> GetBlobsNames()
        {
            return await _blobExplorerService.GetBlobsNames();
        }

        [HttpPost("upload-file"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile([FromForm]IFormFile file)
        {
            try
            {
                var pathToSave = GetDataDirectoryFullPath();

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    await _blobExplorerService.UploadBlob(fullPath, fileName, fileName);
                    return Ok(new { fullPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost("download")]
        public async Task<IActionResult> TestDownload()
        {
            await _blobExplorerService.DownloadBlob();
            return Ok();
        }

        [HttpDelete("delete-all")]
        public async Task<IActionResult> DeleteAllBlobs()
        {
            await _blobExplorerService.DeleteAllBlobs(GetDataDirectoryFullPath());
            return Ok();
        }

        public string GetDataDirectoryFullPath()
        {
            var folderName = "data";
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            return pathToSave;
        }
    }
}
