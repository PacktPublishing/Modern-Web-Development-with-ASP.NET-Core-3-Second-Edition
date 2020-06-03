using chapter18.Models;
using chapter18.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace chapter18
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
            //uncomment the next line for the hosted service sample
            //services.AddSingleton<IHostedService, BackgroundHostedService>();

            services
                .AddMvc(options =>
                {
                    options.Conventions.Add(new CustomConvention());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.Configure<RewriteOptions>(options =>
            {
                options.AddRedirect("redirect-rule/(.*)", "redirected/$1", StatusCodes.Status307TemporaryRedirect);
            });

            services.AddDbContext<OrdersContext>(options =>
            {
                options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<BlogContext>(options =>
            {
                options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddSingleton<PingPongService>();

            services.AddGrpc(options =>
            {
                options.Interceptors.Add<LogInterceptor>();
            });

            services.AddHttpContextAccessor();

            services
                .AddHttpClient("service1", (serviceProvider, client) =>
                {
                    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                    var url = configuration["Services:Service1:Url"];
                    client.BaseAddress = new Uri(url);
                })
                .AddHttpMessageHandler<UserIdHandler>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.AddHttpClient<IOpenWeatherMap, OpenWeatherMap>("OpenWeatherMap", client =>
            {
                client.BaseAddress = new Uri("https://samples.openweathermap.org");
            });

            services.AddTransient<DelegatingHandler, UserIdHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime events)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var options = new RewriteOptions()
                .AddRedirectToHttps();

            //app.UseRewriter(options);

            events.ApplicationStarted.Register(state =>
            {
                //application started
                var appParameter = state as IApplicationBuilder;
            }, app);

            events.ApplicationStopping.Register(
                callback: state =>
                {
                    //application is stopping
                },
                state: "some state");

            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/resources",
                FileProvider = new EmbeddedFileProvider(Assembly.GetEntryAssembly())
             });

            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/files",
                FileProvider = new PhysicalFileProvider(Path.GetFullPath("wwwroot")),
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Add("X-SENDER", "ASP.NET Core");
                }
            });

            app.UseDefaultFiles(new DefaultFilesOptions
            {
                DefaultFileNames = new[] { "document.html" },
                RequestPath = "/files",
                FileProvider = new PhysicalFileProvider(Path.GetFullPath("wwwroot")),
            });

            /*var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".text"] = "text/plain";

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider,
                DefaultContentType = "text/plain",
                ServeUnknownFileTypes = true
            });*/

            app.UseDirectoryBrowser("/files");
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                RequestPath = "/resources",
                FileProvider = new EmbeddedFileProvider(Assembly.GetEntryAssembly())
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}");
                endpoints.MapGrpcService<PingPongService>();
            });           
        }
    }
}
