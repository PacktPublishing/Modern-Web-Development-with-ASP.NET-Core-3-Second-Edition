using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace chapter10
{
    public class LogFilter : IAsyncActionFilter
    {
        private readonly ILoggerFactory _loggerFactory;

        public LogFilter(ILoggerFactory loggerFactory)
        {
            this._loggerFactory = loggerFactory;
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var logger = this._loggerFactory.CreateLogger(context.Controller.GetType());
            logger.LogTrace($"{context.ActionDescriptor.DisplayName} action called");
            return next();
        }
    }
}