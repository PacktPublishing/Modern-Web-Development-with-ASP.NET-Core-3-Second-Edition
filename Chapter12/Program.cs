using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace chapter12
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
                .ConfigureLogging(builder =>
                {
                    builder
                        .AddConsole()
                        .AddDebug();

                })
                .ConfigureWebHostDefaults(builder =>
                {
                    Activity.DefaultIdFormat = ActivityIdFormat.W3C;
                    Activity.ForceDefaultIdFormat = true;

                    builder.UseIISIntegration();
                    builder.UseKestrel(options =>
                    {
                        options.AllowSynchronousIO = true;
                    });
                    builder.UseStartup<Startup>();
                });

    }
}
