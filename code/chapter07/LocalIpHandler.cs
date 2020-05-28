using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace chapter07
{
    public sealed class LocalIpHandler : AuthorizationHandler<LocalIpRequirement>
    {
        public LocalIpHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.HttpContext = httpContextAccessor.HttpContext;
        }

        public HttpContext HttpContext { get; }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            LocalIpRequirement requirement)
        {
            var success = IPAddress.IsLoopback(this.HttpContext.Connection.RemoteIpAddress);

            if (success)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
