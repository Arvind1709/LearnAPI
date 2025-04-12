using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LearnAPI.AppStart
{
    public class CustomAuthorizationFilter : IAuthorizationFilter
    {
        //public void OnAuthorization(AuthorizationFilterContext context)
        //{
        //    var user = context.HttpContext.User;
        //    if (!user.Identity.IsAuthenticated || !user.IsInRole("Admin"))
        //    {
        //        context.Result = new ForbidResult();
        //    }
        //}

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var endpoint = context.HttpContext.GetEndpoint();
            var actionName = context.RouteData.Values["action"]?.ToString();
            var controllerName = context.RouteData.Values["controller"]?.ToString();

            // 🔥 Skip this filter for Login action
            if (controllerName == "Auth" && actionName == "Login")
                return;

            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated || !user.IsInRole("Admin"))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
