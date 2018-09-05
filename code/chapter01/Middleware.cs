using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace chapter01
{
    public class Middleware
    {
        private readonly RequestDelegate _next;

        public Middleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync("This is a middleware class!");
            await this._next(context);
        }
    }
}
