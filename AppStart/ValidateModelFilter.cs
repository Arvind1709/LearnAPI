using LearnAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LearnAPI.AppStart
{
    public class ValidateModelFilter : IActionFilter
    {

        //public void OnActionExecuting(ActionExecutingContext context)
        //{
        //    if (!context.ModelState.IsValid)
        //    {
        //        context.Result = new BadRequestObjectResult(context.ModelState);
        //    }
        //}

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Step 1: Validate the Model
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }

            // Step 2: Modify incoming BookModel if exists
            if (context.ActionArguments.ContainsKey("bookModel"))
            {
                var book = context.ActionArguments["bookModel"] as BookModel;
                //var book1 = context.
                if (book != null)
                {
                    // Example modification: Set minimum price if invalid
                    if (book.Price <= 0)
                    {
                        book.Price = 100; // Default price
                    }

                    // Optional: Trim Title and Author
                    book.Title = book.Title?.Trim();
                    book.Author = book.Author?.Trim();
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
