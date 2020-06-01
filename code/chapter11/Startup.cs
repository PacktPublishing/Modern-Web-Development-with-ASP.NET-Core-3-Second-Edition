using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Hosting;

namespace chapter11
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
            services.AddSingleton<IMyService, MyService>();

            services.AddAuthentication(options =>
            {
            });
            services.AddAuthorization(options =>
            {
            });

            services
                .AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.RootDirectory = "/Pages";
                    //default page
                    options.Conventions.AddPageRoute("/HelloWorld", "");
                    options.Conventions.AuthorizeAreaPage("Admin", "/Index");
                    options.Conventions.AllowAnonymousToPage("/HelloWorld");
                    options.Conventions.AddPageRoute("/Order", "My/Order/{id:int}");
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
