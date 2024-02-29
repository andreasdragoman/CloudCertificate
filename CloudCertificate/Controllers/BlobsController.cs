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

        [HttpPost("upload")]
        public async Task<IActionResult> TestUpload()
        {
            await _blobExplorerService.UploadBlob();
            return Ok();
        }

        [HttpPost("upload-file"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile([FromForm]IFormFile file)
        {
            try
            {
                var folderName = "data";
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    await _blobExplorerService.UploadBlob(pathToSave, fileName, fileName, fullPath);
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
            var folderName = "data";
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            await _blobExplorerService.DeleteAllBlobs(pathToSave);
            return Ok();
        }
    }
}
