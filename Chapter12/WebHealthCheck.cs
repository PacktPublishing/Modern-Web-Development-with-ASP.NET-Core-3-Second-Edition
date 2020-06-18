using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace chapter12
{
    public class WebHealthCheck : IHealthCheck
    {
        public WebHealthCheck(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            this.Url = url;
        }

        public string Url { get; }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            var client = new HttpClient();
            var response = await client.GetAsync(this.Url);

            if (response.StatusCode < HttpStatusCode.BadRequest)
            {
                return HealthCheckResult.Healthy("The URL is up and running");
            }

            return HealthCheckResult.Unhealthy("The URL is inaccessible");
        }
    }
}