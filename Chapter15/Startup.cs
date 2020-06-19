using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using CoreProfiler.Web;
using CoreProfiler;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System.Threading;
using StackExchange.Profiling.SqlFormatters;

namespace chapter15
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services
                .AddMiniProfiler(options =>
                {
                    options.RouteBasePath = "/profiler";
                    options.SqlFormatter = new VerboseSqlServerFormatter();
                    options.StartProfiler();
                })
                .AddEntityFramework();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ProfilingSession.Start("session" + Thread.CurrentThread.ManagedThreadId);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCoreProfiler(drillDown: true);

            app
                .UseMiniProfiler()
               .UseStaticFiles()
               .UseRouting()
               .UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}
