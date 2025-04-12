using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LearnAPI.AppStart
{
    public class CustomResultFilter : IResultFilter
    {
        //public void OnResultExecuting(ResultExecutingContext context)
        //{
        //    context.HttpContext.Response.Headers.Add("X-Custom-Header", "MyValue");
        //}

        //public void OnResultExecuted(ResultExecutedContext context)
        //{
        //    // After result execution
        //}

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult result)
            {
                context.Result = new ObjectResult(new
                {
                    success = true,
                    data = result.Value
                })
                {
                    StatusCode = result.StatusCode
                };
            }
        }

        public void OnResultExecuted(ResultExecutedContext context) { }
    }
}
