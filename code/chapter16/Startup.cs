using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Hosting;
using chapter16.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Connections;
using System.Threading;
using System;
using Microsoft.AspNetCore.SignalR;

namespace chapter16
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
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddRazorRuntimeCompilation();

            services
                .AddSignalR()
                .AddMessagePackProtocol();

            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins("http://localhost:5000", "https://localhost:5001")
                    .AllowCredentials();
            }));

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            TimerCallback callback = (x) =>
            {
                var hub = app.ApplicationServices.GetService<IHubContext<TimerHub>>();
                hub.Clients.All.SendAsync("notify");
            };

            var timer = new Timer(callback);
            timer.Change(
                dueTime: TimeSpan.FromSeconds(0), 
                period: TimeSpan.FromSeconds(5));

            app.UseAuthentication();
            app.UseCors();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHub<ChatHub>("chat", opt =>
                {
                    opt.Transports = HttpTransportType.ServerSentEvents | HttpTransportType.LongPolling | HttpTransportType.WebSockets;
                });
                endpoints.MapHub<TimerHub>("timer");
            });
        }
    }
}
