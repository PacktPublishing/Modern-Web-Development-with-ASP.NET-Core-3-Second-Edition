using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace chapter01
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .UseKestrel((context, options) =>
                {
                    options.Limits.MaxConcurrentConnections = 10;
                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    //add or remove from the configuration builder
                })
                .ConfigureLogging((context, builder) =>
                {
                    //add or remove from the logging builder
                })
                .ConfigureServices((context, builder) =>
                {
                    //register services
                })
                //.UseStartup(typeof(Startup).Assembly.FullName);
                .UseStartup<Startup>();
    }
}
