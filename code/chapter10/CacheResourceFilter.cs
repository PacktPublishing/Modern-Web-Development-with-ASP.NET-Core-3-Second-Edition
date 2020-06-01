using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace chapter10
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class CacheResourceFilter : Attribute, IResourceFilter
    {
        public TimeSpan Duration { get; }

        public CacheResourceFilter(TimeSpan duration)
        {
            this.Duration = duration;
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            var cacheKey = context.HttpContext.Request.Path.ToString().ToLowerInvariant();
            var memoryCache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();

            var result = context.Result as ContentResult;

            if (result != null)
            {
                memoryCache.Set(cacheKey, result.Content, this.Duration);
            }
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var cacheKey = context.HttpContext.Request.Path.ToString().ToLowerInvariant();
            var memoryCache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();

            if (memoryCache.TryGetValue(cacheKey, out var cachedValue))
            {
                if (cachedValue != null && cachedValue is string cachedValueString)
                {
                    context.Result = new ContentResult() { Content = cachedValueString };
                }
            }
        }
    }
}
