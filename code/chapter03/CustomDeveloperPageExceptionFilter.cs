using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace chapter03
{
    class CustomDeveloperPageExceptionFilter : IDeveloperPageExceptionFilter
    {
        public async Task HandleExceptionAsync(ErrorContext errorContext, Func<ErrorContext, Task> next)
        {
            if (errorContext.Exception is DbException)
            {
                await errorContext.HttpContext.Response.WriteAsync("Error connecting to the DB");
            }
            else
            {
                await next(errorContext);
            }
        }
    }
}