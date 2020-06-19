using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using System.Collections.Generic;
using System.Linq;

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

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services
                .AddFeatureManagement()
                .UseDisabledFeaturesHandler(new RedirectDisabledFeatureHandler("/Home/FeatureDisabled"));

            var token = this.Configuration.GetReloadToken();
            token.RegisterChangeCallback(callback: (state) =>
            {
                //state will be someData
                //push the changes to whoever needs it
            }, state: "SomeData");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var keysAndValues = this.Configuration
                .AsEnumerable()
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            var settings = new LoggingSettings { LogLevel = new Dictionary<string, LogLevel>() };

            this.Configuration
                .GetSection("Logging")
                .Bind(settings);

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
