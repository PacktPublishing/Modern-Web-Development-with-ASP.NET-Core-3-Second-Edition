using Microsoft.AspNetCore.Mvc.Filters;

namespace chapter10
{
    public class FirstFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Items["WasFirstFilterExecuted"] = true;
        }
    }

    public class SecondFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Items["WasFirstFilterExecuted"] is bool parameter && parameter)
            {
                //proceed accordingly
            }
        }
    }
}
