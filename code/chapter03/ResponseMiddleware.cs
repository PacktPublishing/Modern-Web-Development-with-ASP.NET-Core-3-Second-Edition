using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace chapter03
{
    internal class ResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext ctx)
        {
            await ctx.Response.WriteAsync("Hello, from a middleware!");
        }
    }
}