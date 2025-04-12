using Microsoft.AspNetCore.Mvc;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        //[HttpGet("logs")]
        //public IActionResult GetLogs()
        //{
        //    var logFile = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "log-20250321.txt");
        //    if (!System.IO.File.Exists(logFile))
        //        return NotFound("Log file not found.");

        //    var content = System.IO.File.ReadAllText(logFile);
        //    return Content(content, "text/plain");
        //}
        [HttpGet("logs/{date}")]
        public IActionResult GetLogs(string date)  // Expected format: 20250321
        {
            var logFile = Path.Combine(Directory.GetCurrentDirectory(), "Logs", $"log-{date}.txt");
            if (!System.IO.File.Exists(logFile))
                return NotFound("Log file not found.");

            var content = System.IO.File.ReadAllText(logFile);
            return Content(content, "text/plain");
        }
    }


}
