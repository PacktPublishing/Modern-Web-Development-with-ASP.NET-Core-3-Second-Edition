using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            services.AddSingleton<ITranslator, MyTranslator>();
            services.AddSingleton<IDeveloperPageExceptionFilter, CustomDeveloperPageExceptionFilter>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePagesWithRedirects("/error/{0}");
            //app.UseStatusCodePages("text/html", "Error: {0}");

            app.UseRouting();


            app.UseStatusCodePages(async context =>
            {
                context.HttpContext.Response.ContentType = "text/plain";
                var statusCode = context.HttpContext.Response.StatusCode;
                await context.HttpContext.Response.WriteAsync("HTTP status code: " + statusCode);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet(
                    pattern: "DirectRoute",
                    requestDelegate: async ctx =>
                    {
                        ctx.Response.ContentType = "text/plain";
                        await ctx.Response.WriteAsync("Here's your response!");
                    });

                var newAppbuilder = endpoints.CreateApplicationBuilder();
                newAppbuilder.UseMiddleware<ResponseMiddleware>();

                endpoints.MapGet("DirectMiddlewareRoute", newAppbuilder.Build());

                endpoints.MapDynamicControllerRoute<TranslateRouteValueTransformer>(
                    pattern: "{language}/{controller}/{action}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithDisplayName("Foo")
                .WithMetadata(new MyMetadata1(), new MyMetadata2());

                endpoints.MapControllerRoute("Local", "Home/Local").RequireHost("localhost", "127.0.0.1");

                endpoints.MapControllerRoute(
                    name: "Error404",
                    pattern: "error/404",
                    defaults: new { controller = "CatchAll", action = "Error404" }
                );

                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
