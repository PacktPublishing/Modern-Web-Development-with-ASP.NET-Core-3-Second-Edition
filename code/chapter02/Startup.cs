using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace chapter02
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;

            var cfg = configuration;

            cfg["key"] = "value";
            string value = cfg["key"];

            var section = cfg.GetSection("ConnectionStrings");
            var exists = section.Exists();

            var valueFromRoot = cfg["A:B:C"];
            var aSection = cfg.GetSection("A");
            var bSection = aSection.GetSection("B");
            var valueFromSection = bSection["C"];

            var loggingSettings = cfg.GetSection("Logging").Get<LoggingSettings>();

            var defaultConnection = cfg["ConnectionStrings:DefaultConnection"];

            var token = cfg.GetReloadToken();
            token.RegisterChangeCallback(callback: (state) =>
            {
                //state will be someData
                //push the changes to whoever needs it
            }, state: "SomeData");

            (cfg as IConfigurationRoot).Reload();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureOptions<PreConfigureLoggingSettings>();
            services.ConfigureOptions<PreConfigureNamedLoggingSettings>();

            services.Configure<LoggingSettings>(this.Configuration.GetSection("Logging"));

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvcWithDefaultRoute();
        }
    }
}
