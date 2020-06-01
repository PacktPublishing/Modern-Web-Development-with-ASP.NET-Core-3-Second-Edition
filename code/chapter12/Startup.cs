using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.Elm;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Text.Json;

namespace chapter12
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
            services.AddSession();
            services.AddHttpContextAccessor();


            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddLogging(options =>
            {
                options.AddFilter((category, logLevel) => logLevel >= LogLevel.Warning);
            });

            services.AddMiddlewareAnalysis();
            services.AddElm();

            services.Configure<ElmOptions>(options =>
            {
                options.Path = "/_Elm";
                options.Filter = (name, logLevel) =>
                {
                    return logLevel > LogLevel.Information;
                };
            });

            services.AddSingleton<TraceIdentifierMessageHandler>();

            services
                .AddHttpClient("chapter12", (serviceProvider, client) =>
                {
                    var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                    var traceIdentifier = httpContext.TraceIdentifier;  //just to check
                    var host = httpContext.Request.Host;
                    var proto = httpContext.Request.Scheme;

                    client.BaseAddress = new Uri($"{proto}://{host}");
                })
                .AddHttpMessageHandler<TraceIdentifierMessageHandler>();


            services
                .AddHealthChecks()
                .AddCheck("Web Check", new WebHealthCheck("http://google.com"), HealthStatus.Unhealthy)
                .AddCheck("Sample Lambda", () => HealthCheckResult.Healthy("All is well!"))
                .AddDbContextCheck<MyDbContext>("My Context"); //check a database through EF Core
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DiagnosticListener diagnosticListener)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var listener = new TraceDiagnosticListener();
            diagnosticListener.SubscribeWithAdapter(listener);

            app.UseAuthentication();
            app.UseRouting();
            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseElmPage();
                app.UseElmCapture();
            }

            app.Properties["analysis.NextMiddlewareName"] = "MyCustomMiddleware";
            app.Use(async (context, next) =>
            {
                //do something
                await next();
            });

            app.UseMiddleware<LoggingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints
                    .MapHealthChecks("/health", new HealthCheckOptions
                    {
                        ResponseWriter = async (context, report) =>
                        {
                            var result = JsonSerializer.Serialize(new
                            {
                                Status = report.Status.ToString(),
                                Checks = report.Entries.Select(e => new
                                {
                                    Check = e.Key,
                                    Status = e.Value.Status.ToString()
                                })
                            });
                            context.Response.ContentType = MediaTypeNames.Application.Json;
                            await context.Response.WriteAsync(result);
                        }
                    })
                    .RequireHost("localhost");
                    //.RequireAuthorization();
            });
        }
    }
}
