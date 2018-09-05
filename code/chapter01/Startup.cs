using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace chapter01
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MyType>();

            services.AddScoped<IMyService, MyService>();

            services.AddScoped<IMyOtherService, MyOtherService>();

            services.AddHttpContextAccessor();

            /*
            services.AddScoped<IMyService>(sp =>
              new MyService((IMyOtherService)sp.GetService(typeof(IMyOtherService))));
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

            //build the service provider
            var serviceProvider = services.BuildServiceProvider();

            //retrieve a constructed generic type
            var myGenericService = serviceProvider.GetService<MyGenericService<string>>();

            var factory = serviceProvider.GetService<IServiceScopeFactory>();

            using (var scope = factory.CreateScope())
            {
                var svc = scope.ServiceProvider.GetService<IMyService>();
            }

            //AutoFac
            var builder = new ContainerBuilder();
            //add registrations from services
            builder.Populate(services);
            return new AutofacServiceProvider(builder.Build());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
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
            });

            //app.UseMiddleware<Middleware>();
            //app.Use(Process);

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello, OWIN World!");
            });
        }

        async Task Process(HttpContext context, Func<Task> next)
        {
            await context.Response.WriteAsync("Step 1");
            await next();
        }
    }
}
