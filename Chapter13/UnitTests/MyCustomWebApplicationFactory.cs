using chapter13;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    class MyCustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return base
                .CreateHostBuilder()
                .ConfigureServices(services =>
                {
                    services.RemoveAll<IHostedService>();
                });
        }
    }
}
