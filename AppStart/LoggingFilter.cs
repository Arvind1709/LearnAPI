using Microsoft.AspNetCore.Mvc.Filters;

namespace LearnAPI.AppStart
{
    public class LoggingFilter : IActionFilter
    {
        //public void OnActionExecuting(ActionExecutingContext context)
        //{
        //    Debug.WriteLine($"[LOG] Action {context.ActionDescriptor.DisplayName} started at {DateTime.Now}");
        //}

        //public void OnActionExecuted(ActionExecutedContext context)
        //{
        //    Debug.WriteLine($"[LOG] Action {context.ActionDescriptor.DisplayName} finished at {DateTime.Now}");
        //}
        private readonly ILogger<LoggingFilter> _logger;
        public LoggingFilter(ILogger<LoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"Executing: {context.ActionDescriptor.DisplayName}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"Executed: {context.ActionDescriptor.DisplayName} with Status Code {context.HttpContext.Response.StatusCode}");
        }
    }
}
