using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace chapter03
{
    public class IsAuthenticatedRouteConstraint : IRouteConstraint
    {
        public bool Match(
            HttpContext httpContext,
            IRouter route,
            string routeKey,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            return httpContext.Request.Cookies.ContainsKey("auth");
        }
    }
}
