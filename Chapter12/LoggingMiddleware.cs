using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chapter12
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;

        public LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this._next = next;
            this._loggerFactory = loggerFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var logger = this._loggerFactory.CreateLogger<LoggingMiddleware>();
            using (logger.BeginScope<LoggingMiddleware>(this))
            {
                logger.LogInformation("Before request");
                await this._next.Invoke(context);
                logger.LogInformation("After request");
            }
        }
    }
}
