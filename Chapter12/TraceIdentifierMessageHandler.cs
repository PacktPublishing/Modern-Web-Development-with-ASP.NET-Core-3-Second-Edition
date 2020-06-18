using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace chapter12
{
    public class TraceIdentifierMessageHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TraceIdentifierMessageHandler(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpContext = this._httpContextAccessor.HttpContext;
            request.Headers.Add("Request-Id", httpContext.TraceIdentifier);
            request.Headers.Add("X-SessionId", httpContext.Session.Id);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
