using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using System.IO;

namespace chapter14
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
            services.AddNodeServices();
            services.AddSpaStaticFiles(options =>
            {
                options.RootPath = Path.GetFullPath("wwwroot");
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.Use(async (ctx, next) =>
            {
                var nodeServices = ctx.RequestServices.GetRequiredService<INodeServices>();
                var result = await nodeServices.InvokeAsync<int>("ClientApp/Algebra.js", 10, 20);

                ctx.Response.ContentType = "text/html";
                await ctx.Response.WriteAsync("<html><body>");
                await ctx.Response.WriteAsync($"<p>Result is: {result}<br/>Try to navigate to <a href=\"http://localhost:3000\">Express</a></p>");
                await ctx.Response.WriteAsync("</body></html>");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

        }
    }
}
