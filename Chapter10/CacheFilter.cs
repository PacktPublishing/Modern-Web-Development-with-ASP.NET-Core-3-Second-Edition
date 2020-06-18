using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace chapter10
{
    public class CacheFilter : IResultFilter
    {
        private readonly IMemoryCache _cache;

        public CacheFilter(IMemoryCache cache)
        {
            this._cache = cache;
        }

        private object GetKey(ActionDescriptor action)
        {
            //generate a key and return it
            return action.Id;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            var key = this.GetKey(context.ActionDescriptor);
            string html;

            if (this._cache.TryGetValue<string>(key, out html))
            {
                context.Result = new ContentResult { Content = html, ContentType = "text/html" };
            }
            else
            {
                if (context.Result is ViewResult)
                {
                    //get the rendered view, maybe using a TextWriter, and store it in the cache 
                }
            }
        }
    }
}
