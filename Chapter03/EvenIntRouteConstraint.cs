using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace chapter03
{
    public class EvenIntRouteConstraint : IRouteConstraint
    {
        public bool Match(
            HttpContext httpContext,
            IRouter route,
            string routeKey,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            if ((values.ContainsKey(routeKey) == false) || (values[routeKey] == null))
            {
                return false;
            }

            var value = values[routeKey].ToString();

            if (int.TryParse(value, out var intValue) == false)
            {
                return false;
            }

            return (intValue % 2) == 0;
        }
    }
}