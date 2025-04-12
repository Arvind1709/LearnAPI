using Microsoft.AspNetCore.Mvc.Filters;

namespace LearnAPI.AppStart
{
    public class ActionLoggingFilter : IActionFilter
    {
        //public void OnActionExecuting(ActionExecutingContext context)
        //{
        //    // Logic before the action
        //}

        //public void OnActionExecuted(ActionExecutedContext context)
        //{
        //    // Logic after the action
        //}
        private readonly ILogger<ActionLoggingFilter> _logger;
        public ActionLoggingFilter(ILogger<ActionLoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var actionArguments = string.Join(", ", context.ActionArguments.Select(kv => $"{kv.Key}: {kv.Value}"));
            _logger.LogInformation($"Action arguments: {actionArguments}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Action execution finished");
        }
    }
}
