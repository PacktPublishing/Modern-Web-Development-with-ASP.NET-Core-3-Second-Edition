using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;

namespace chapter02
{
    public static class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
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

                    builder.AddUserSecrets("9094c8e7-0000-0000-0000-c26798dc18d2");

                    var switchMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { { "--Key1", "AnotherKey" } };

                    builder.AddCommandLine(
                        args: Environment.GetCommandLineArgs().Skip(1).ToArray(),
                        switchMappings: switchMappings
                        );

                    var properties = new Dictionary<string, string> { { "key", "value" } };
                    builder.AddInMemoryCollection(properties);

                    AppDomain.CurrentDomain.SetData("Foo", "ReBar");

                    var bar = AppContext.GetData("Foo");

                    //builder.AddRegistry(RegistryHive.LocalMachine);

                })
            //.ConfigureHostConfiguration(builder => { })
            .ConfigureWebHostDefaults(builder =>
            {
                builder
                //.UseSetting("key", "value")
                .UseStartup<Startup>();
            });
    }
}
