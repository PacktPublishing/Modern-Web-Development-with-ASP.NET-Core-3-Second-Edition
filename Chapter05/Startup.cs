using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace chapter05
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
            services.AddSingleton<IRazorPageActivator, CustomRazorPageActivator>();

            services
                .AddLocalization(options => options.ResourcesPath = "Resources");

            services
                .AddMvc()
                .AddMvcLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddViewLocalization
                (
                    format: LanguageViewLocationExpanderFormat.Suffix,
                    setupAction: options =>
                    {
                        options.ResourcesPath = "Resources";
                    }
                )
                .AddRazorRuntimeCompilation()
                .AddRazorOptions(options =>
                {
                    options.ViewLocationExpanders.Add(new ThemesViewLocationExpander("Mastering"));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        public void Configure(IApplicationBuilder app)
        {
            var supportedCultures = new[] { "en", "pt" };

            var localizationOptions = new RequestLocalizationOptions()
                //.AddInitialRequestCultureProvider(new AcceptLanguageHeaderRequestCultureProvider())
                .SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
