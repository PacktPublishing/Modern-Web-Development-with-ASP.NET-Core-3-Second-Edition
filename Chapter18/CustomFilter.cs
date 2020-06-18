using Microsoft.AspNetCore.Mvc.Filters;

namespace chapter18
{
    internal class CustomFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}