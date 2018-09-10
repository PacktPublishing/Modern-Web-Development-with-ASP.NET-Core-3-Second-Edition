using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

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

            services.AddMvc()
              .AddRazorPagesOptions(options =>
              {
                  options.AllowAreas = true;
                  options.AllowMappingHeadRequestsToGetHandler = true;
                  options.RootDirectory = "/Pages";
                  options.Conventions.AddPageRoute("/HelloWorld", "");
                  options.Conventions.AuthorizeAreaPage("Admin", "/Index");
                  options.Conventions.AllowAnonymousToPage("/HelloWorld");
                  options.Conventions.AddPageRoute("/Order", "My/Order/{id:int}");
              });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
