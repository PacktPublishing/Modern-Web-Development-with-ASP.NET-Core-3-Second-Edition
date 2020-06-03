using chapter18.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace chapter18
{
    public static class HostExtensions
    {
        public static IHost CreateDbIfNotExists(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    var ordersContext = services.GetRequiredService<OrdersContext>();
                    var ordersCreated = ordersContext.Database.EnsureCreated();
                    logger.LogInformation("Orders DB created successfully: {created}.", ordersCreated);

                    var blogContext = services.GetRequiredService<BlogContext>();
                    var blogCreated = blogContext.Database.EnsureCreated();
                    logger.LogInformation("Blog DB created successfully: {created}.", blogCreated);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while creating the DB.");
                }
            }

            return host;
        }
    }


    public class Program
    {
        

        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                //.CreateDbIfNotExists()
                .Run();
        }

        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
                .CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    //services.AddHostedService<BackgroundService>();
                })
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.ConfigureKestrel(options =>
                    {
                        //uncomment the next line for the Grpc sample
                        //options.ListenLocalhost(5000, o => o.Protocols = HttpProtocols.Http2);
                    });
                    builder.UseStartup<Startup>();
                });

    }
}
