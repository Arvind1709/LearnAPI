using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LearnAPI.AppStart
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        //public void OnException(ExceptionContext context)
        //{
        //    context.Result = new ObjectResult(new
        //    {
        //        Message = "An unexpected error occurred.",
        //        Error = context.Exception.Message
        //    })
        //    {
        //        StatusCode = 500
        //    };
        //}
        private readonly ILogger<GlobalExceptionFilter> _logger;
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Unhandled exception");
            context.Result = new ObjectResult(new { message = "An unexpected error occurred. Please try again later." })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
