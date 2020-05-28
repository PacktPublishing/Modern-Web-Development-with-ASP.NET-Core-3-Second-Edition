using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace chapter07
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host
                .CreateDefaultBuilder(args)
                .ConfigureLogging((ctx, builder) =>
                {
                    if (ctx.HostingEnvironment.IsDevelopment())
                    {
                        builder
                        .AddConsole()
                        .AddDebug();
                    }
                })
                .ConfigureAppConfiguration((ctx, builder) =>
                {
                
                })
                .ConfigureWebHostDefaults(builder =>
                {
                    

                    builder.UseStartup<Startup>();
                });
    }
}
