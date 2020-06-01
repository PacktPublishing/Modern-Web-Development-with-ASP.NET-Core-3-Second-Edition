using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chapter12
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseConventionalMiddleware<T>(this IApplicationBuilder builder, params object[] args)
        {
            return builder.UseMiddleware<T>(args);
        }

        public static IApplicationBuilder UseFactoryActivatedMiddleware<T>(this IApplicationBuilder builder, params object[] args) where T : IMiddleware
        {
            return builder.UseMiddleware<T>(args);
        }
    }
}
