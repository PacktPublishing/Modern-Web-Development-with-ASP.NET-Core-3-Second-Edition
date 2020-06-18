using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace chapter18
{
    public class UserIdHandler : DelegatingHandler
    {
        public UserIdHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.HttpContext = httpContextAccessor.HttpContext;
        }

        protected HttpContext HttpContext { get; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            request.Headers.Add("UserId", this.HttpContext.User.Identity.Name);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
