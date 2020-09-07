using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace chapter01
{
    public class Middleware
    {
        private readonly RequestDelegate _next;

        public Middleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync("This is a middleware class!");
            await _next(context);
        }
    }
}
