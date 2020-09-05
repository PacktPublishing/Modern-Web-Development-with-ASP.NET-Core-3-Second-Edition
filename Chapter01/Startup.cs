using System;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace chapter01
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        // ConfigureServices is where you register dependencies. This gets
        // called by the runtime before the ConfigureContainer method, below.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the collection. Don't build or return
            // any IServiceProvider or the ConfigureContainer method
            // won't get called. Don't create a ContainerBuilder
            // for Autofac here, and don't call builder.Populate() - that
            // happens in the AutofacServiceProviderFactory for you.
            services.AddOptions();

            services.AddSingleton<MyType>()
                .AddScoped<IMyService, MyService>()
                .AddScoped<IMyOtherService, MyOtherService>();

            services.AddHttpContextAccessor();

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            /*
            services.AddScoped<IMyService>(sp => new MyService((IMyOtherService)sp.GetService(typeof(IMyOtherService))));
            services.AddScoped(typeof(IMyService), typeof(MyService));
            services.Add(new ServiceDescriptor(typeof(IMyService), typeof(MyService), ServiceLifetime.Scoped));

            //for a scoped registration
            services.Add(new ServiceDescriptor(typeof(IMyService), typeof(MyService), ServiceLifetime.Scoped);

            //for singleton, both work
            services.Add(new ServiceDescriptor(typeof(IMyService), typeof(MyService),
                ServiceLifetime.Singleton);
            services.Add(new ServiceDescriptor(typeof(IMyService), new MyService());

            //with a factory that provides the service provider as a parameter, from which you can retrieve //other services
            services.Add(new ServiceDescriptor(typeof(IMyService), (serviceProvider) =>
                new MyService(), ServiceLifetime.Transient);
            */

            //register an open generic type
            services.AddScoped(typeof(MyGenericService<>));
        }
        /*
        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
            builder.RegisterModule(new MyApplicationModule());
        }
        */

        // Configure is where you add middleware. This is called after
        // ConfigureContainer. You can use IApplicationBuilder.ApplicationServices
        // here if you need to resolve things from the container.
        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            var instance = ActivatorUtilities.CreateInstance<MyType>(serviceProvider);

            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("step 1!");
                await next();
            });

            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("step 2!");
                await next();
            });

            //app.UseMiddleware<Middleware>();
            //app.Use(Process);

            app.Run(async (context) => await context.Response.WriteAsync("Hello, OWIN World!"));
        }
        /*
            private async Task Process(HttpContext context, Func<Task> next)
            {
                await context.Response.WriteAsync("Step 3");
                await next();
            }
        */
    }
}
