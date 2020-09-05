using Microsoft.AspNetCore.Http;

namespace chapter01
{
    public class MyType
    {
        private readonly HttpContext httpContext;
        public MyType(IHttpContextAccessor httpContextAccessor) => httpContext = httpContextAccessor.HttpContext;
    }
}