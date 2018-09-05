using Microsoft.AspNetCore.Http;

namespace chapter01
{
    public class MyType
    {
        public MyType(IHttpContextAccessor httpContextAccessor)
        {
            var httpContext = httpContextAccessor.HttpContext;
        }
    }
}