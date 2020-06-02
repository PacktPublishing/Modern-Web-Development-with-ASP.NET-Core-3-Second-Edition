using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chapter07
{
    public class ProtectedPathsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ProtectedPathOptions _options;

        public ProtectedPathsMiddleware(
            RequestDelegate next,
            ProtectedPathOptions options)
        {
            this._next = next;
            this._options = options;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (context.RequestServices.CreateScope())
            {
                var authSvc = context.RequestServices.GetRequiredService<IAuthorizationService>();

                if (context.Request.Path.StartsWithSegments(this._options.Path))
                {
                    var result = await authSvc.AuthorizeAsync(
                        context.User,
                        context.Request.Path,
                        this._options.PolicyName);

                    if (!result.Succeeded)
                    {
                        await context.ChallengeAsync();
                        return;
                    }
                }
            }

            await this._next.Invoke(context);
        }
    }
}