using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace chapter04
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public ValidateModelStateAttribute(string redirectUrl)
        {
            this.RedirectUrl = redirectUrl;
        }

        public ValidateModelStateAttribute(string actionName, string controllerName = null, object routeValues = null)
        {
            this.ControllerName = controllerName;
            this.ActionName = actionName;
            this.RouteValues = routeValues;
        }

        public string RedirectUrl { get; }
        public string ActionName { get; }
        public string ControllerName { get; }
        public object RouteValues { get; }

        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.ModelState.IsValid == false)
            {
                if (string.IsNullOrWhiteSpace(this.RedirectUrl) == false)
                {
                    context.Result = new RedirectResult(this.RedirectUrl);
                }
                else if (string.IsNullOrWhiteSpace(this.ActionName) == false)
                {
                    context.Result = new RedirectToActionResult(this.ActionName, this.ControllerName, this.RouteValues);
                }
                else
                {
                    context.Result = new BadRequestObjectResult(context.ModelState);
                }
            }

            return base.OnResultExecutionAsync(context, next);
        }
    }
}
