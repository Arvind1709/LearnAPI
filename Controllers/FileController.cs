using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using System.Text;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        [HttpGet("download-zip")]
        public IActionResult DownloadZip()
        {
            string fileName = "SampleText.txt";
            string zipFileName = "SampleText.zip";
            string tempPath = Path.GetTempPath();
            string filePath = Path.Combine(tempPath, fileName);
            string zipFilePath = Path.Combine(tempPath, zipFileName);

            // Write some content to the text file
            System.IO.File.WriteAllText(filePath, "Hello, this is a sample text file.\nEnjoy your download!");

            // Create ZIP file
            using (FileStream zipToOpen = new FileStream(zipFilePath, FileMode.Create))
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
            {
                archive.CreateEntryFromFile(filePath, fileName);
            }

            // Read ZIP file and return it
            byte[] zipBytes = System.IO.File.ReadAllBytes(zipFilePath);
            return File(zipBytes, "application/zip", zipFileName);
        }
        [HttpGet("download-zip1")]
        public IActionResult DownloadZip1()
        {
            using (MemoryStream zipStream = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    var entry = archive.CreateEntry("SampleText.txt");
                    using (StreamWriter writer = new StreamWriter(entry.Open(), Encoding.UTF8))
                    {
                        writer.WriteLine("Hello, this is a sample text file.");
                        writer.WriteLine("Enjoy your download!");
                    }
                }

                zipStream.Seek(0, SeekOrigin.Begin);
                return File(zipStream.ToArray(), "application/zip", "SampleText.zip");
            }
        }
        [HttpGet("download-zip2")]
        public IActionResult DownloadZip2()
        {
            string filePath = @"C:\Users\Dell\Desktop\APILearningTools\SampleText.txt";
            string zipFilePath = @"C:\Users\Dell\Desktop\APILearningTools\SampleText.zip";

            System.IO.File.WriteAllText(filePath, "This is a test file.");

            using (FileStream zipToOpen = new FileStream(zipFilePath, FileMode.Create))
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
            {
                archive.CreateEntryFromFile(filePath, "SampleText.txt");
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(zipFilePath);
            return new FileContentResult(fileBytes, "application/zip")
            {
                FileDownloadName = "SampleText.zip"
            };
        }
        [HttpGet("CheckTeapot")]
        public IActionResult CheckTeapot(bool isTeapot)
        {
            if (isTeapot)
                //return StatusCode(418, "I'm a teapot!");
                //return BadRequest("Invalid request");
                return Ok(new { message = "Success" });

            //return NotFound("Resource not found");
            //return Created("/resource/1", new { id = 1 });
            return NoContent();
            //return Ok("Not a teapot.");
            //return BadRequest("Invalid request");      // 400 Bad Request
            // return NotFound("Resource not found");     // 404 Not Found
            // return Ok(new { message = "Success" });    // 200 OK
            // return Created("/resource/1", new { id = 1 }); // 201 Created
            //return NoContent(); // 204 No Content
        }

    }
}
