using LearnAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnTypeController : ControllerBase
    {
        [HttpGet("GetData")]
        public IActionResult GetData()
        {
            var data = new { Id = 1, Name = "Book" };
            var data1 = new BookModel
            {
                Id = 1,
                Title = "Title",
                Author = "Author",
                Price = 400,
                Description = "Description",
                Stock = 200,
                Category = "Category",
                CreatedAt = DateTime.Now,
                BookCover = "BookCover"
            };
            return Ok(data1); // Returns HTTP 200 with data
        }
        [HttpGet("GetMessage")]
        public ActionResult<string> GetMessage()
        {
            return "Hello, World!"; // Returns 200 OK with a string response
        }

        [HttpGet("GetJson")]
        public JsonResult GetJson()
        {
            var data = new { Id = 1, Name = "John" };
            var data1 = new BookModel
            {
                Id = 1,
                Title = "Title",
                Author = "Author",
                Price = 400,
                Description = "Description",
                Stock = 200,
                Category = "Category",
                CreatedAt = DateTime.Now,
                BookCover = "BookCover"
            };
            return new JsonResult(data1);
        }

        [HttpGet("GetContent")]
        public ContentResult GetContent()
        {

            //return Content("This is plain text response", "text/plain");
            var test = "This is plain text response";
            return Content(test, "text/plain");
        }

        [HttpGet("GetFile")]
        public IActionResult GetFile()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes("sample.pdf");
            return File(fileBytes, "application/pdf", "download.pdf");
        }
    }
}
