using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace chapter03
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
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = true;
                options.ConstraintMap.Add("evenint", typeof(EvenIntRouteConstraint));
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            /*app.UseExceptionHandler(options =>
            {
                options.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "text/html";
                        var ex = context.Features.Get<IExceptionHandlerFeature>();
                        if (ex != null)
                        {
                            var err = $"<h1>Error: {ex.Error.Message}</h1>{ex.Error.StackTrace}";
                            await context.Response.WriteAsync(err).ConfigureAwait(false);
                        }
                    });
            });

            app.UseExceptionHandler(new ExceptionHandlerOptions { ExceptionHandlingPath = "/error", ExceptionHandler = context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "text/html";
                    var ex = context.Features.Get<IExceptionHandlerFeature>();
                    var err = $"<h1>Error: {ex.Error.Message}</h1>{ex.Error.StackTrace}";
                    return context.Response.WriteAsync(err);
                }
            });*/

            app.UseStatusCodePagesWithRedirects("/error/{0}");
            //app.UseStatusCodePages("text/html", "Error: {0}");

            app.UseMvc(routes =>
            {
                routes.MapGet(
                    template: "DirectRoute",
                    handler: async ctx =>
                    {
                        ctx.Response.ContentType = "text/plain";
                        await ctx.Response.WriteAsync("Here's your response!");
                    });

                routes.MapMiddlewareGet(
                    template: "DirectMiddlewareRoute",
                    action: appBuilder =>
                    {
                        appBuilder.UseMiddleware<ResponseMiddleware>();
                    });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
