using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.IO;

namespace chapter02
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
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var jsonSource = new JsonConfigurationSource { Path = "appsettings.json" };
                    builder.Add(jsonSource);

                    //same as
                    //builder.AddJsonFile("appsettings.json");

                    jsonSource.OnLoadException = (x) =>
                    {
                        if (x.Exception is FileNotFoundException ex)
                        {
                            Console.Out.WriteLine($"File {ex.FileName} not found");
                            x.Ignore = true;
                        }
                    };

                    //builder.AddRegistry(RegistryHive.LocalMachine);
                })
                .UseSetting("key", "value")
                .UseStartup<Startup>();
    }
}
