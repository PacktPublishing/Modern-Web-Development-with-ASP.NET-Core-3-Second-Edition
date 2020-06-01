using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace chapter10
{
    public sealed class ErrorFilter : IAsyncExceptionFilter
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            context.ExceptionHandled = true;
            await context.HttpContext.Response.WriteAsync($"An error occurred: {context.Exception.Message}");
        }
    }
}
