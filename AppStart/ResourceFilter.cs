using Microsoft.AspNetCore.Mvc.Filters;

namespace LearnAPI.AppStart
{
    public class ResourceFilter : IResourceFilter
    {
        //public void OnResourceExecuting(ResourceExecutingContext context)
        //{
        //    // Logic before model binding
        //}

        //public void OnResourceExecuted(ResourceExecutedContext context)
        //{
        //    // Logic after action executes
        //}
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            // Example: Check for some condition or cache
        }

        public void OnResourceExecuted(ResourceExecutedContext context) { }
    }
}
