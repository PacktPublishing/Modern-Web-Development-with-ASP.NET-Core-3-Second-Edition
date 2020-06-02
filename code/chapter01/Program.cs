using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace chapter01
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
                .CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(builder =>
                {
                    //host configuration (Kestrel or HTTP.sys)
                    builder.Properties["key"] = "value";
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        //.UseStartup(typeof(Startup).Assembly.FullName);
                        .UseStartup<Startup>()
                        .UseKestrel(options =>
                        {
                            options.Limits.MaxConcurrentConnections = 10;
                        });
                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    //add or remove from the configuration builder
                    //app configuration
                    builder.Add(new JsonConfigurationSource { Path = "./configuration.json", Optional = true });
                    builder.Properties["key"] = "value";
                })
                .ConfigureLogging((context, builder) =>
                {
                    //add or remove from the logging builder
                    builder.AddConsole();
                })
                .ConfigureServices(services =>
                {
                    //register services
                    services.AddSingleton<IMyService, MyService>();
                });
    }
}
