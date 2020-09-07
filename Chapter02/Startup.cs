using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace chapter02
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var cfg = configuration;

            cfg["key"] = "value";
            var value = cfg["key"];

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

            services.Configure<LoggingSettings>(Configuration.GetSection("Logging"));

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services
                .AddFeatureManagement()
                .UseDisabledFeaturesHandler(new RedirectDisabledFeatureHandler("/Home/FeatureDisabled"));

            var token = Configuration.GetReloadToken();
            token.RegisterChangeCallback(callback: (state) =>
            {
                //state will be someData
                //push the changes to whoever needs it
            }, state: "SomeData");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var keysAndValues = Configuration
                .AsEnumerable()
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            var settings = new LoggingSettings { LogLevel = new Dictionary<string, LogLevel>() };

            Configuration
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
