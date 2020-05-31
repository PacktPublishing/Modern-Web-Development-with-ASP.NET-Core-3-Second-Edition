using chapter08.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace chapter08
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            //register an entity set of type Order and call it Orders
            var orders = builder.EntitySet<Order>("Orders").EntityType.HasKey(x => x.Id);
            //same for products
            builder.EntitySet<Product>("Products").EntityType.HasKey(x => x.Id);
            //add other entity sets here
            return builder.GetEdmModel();
        }

        public void ConfigureServices(IServiceCollection services)
        {
           
            services
               .AddAuthentication(options =>
               {
                   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
               })
               .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateAudience = false,
                       ValidateIssuer = false,
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("<at-least-16-character-secret-key>")),
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.FromMinutes(5)
                   };
               });


            services
                .AddControllers(options =>
                {
                    options.RespectBrowserAcceptHeader = true;
                    options.FormatterMappings.SetMediaTypeMappingForFormat("xml", "application/xml");
                    options.FormatterMappings.SetMediaTypeMappingForFormat("json", "application/json");
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                })
                .AddXmlSerializerFormatters();

            services.AddApiVersioning();

            services
                .AddOData();               
            
            services.Configure<ApiBehaviorOptions>(options =>
            {
                //options.SuppressModelStateInvalidFilter = true;

                options.InvalidModelStateResponseFactory = (ctx) =>
                {
                    if (ctx.ModelState.ErrorCount > 1)
                    {
                        return new JsonResult(new { Errors = ctx.ModelState.ErrorCount });
                    }
                    return new BadRequestObjectResult(ctx.ModelState);
                };
            });

            services.AddRouting();

            //this does not work well with OData, so it is commented out
            /*services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(2, 0);
                //options.ApiVersionReader = new HeaderApiVersionReader("api-version");
                //options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
            });*/

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API V1",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Email = "rjperes@hotmail.com",
                        Name = "Ricardo Peres",
                        Url = new Uri("http://weblogs.asp.net/ricardoperes")
                    }
                });

                //assume that the XML file will have the same name as the current assembly
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
       
            if (env.IsDevelopment())
            { 
                app.UseDeveloperExceptionPage();
                
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var errorFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var exception = errorFeature.Error;
                        var path = errorFeature.Path;
                        var problemDetails = new ProblemDetails
                        {
                            Instance = $"urn:my:error:{Guid.NewGuid()}",
                            Detail = exception.Message
                        };

                        if (exception is BadHttpRequestException badHttpRequestException)
                        {
                            problemDetails.Title = "Invalid request!";
                            problemDetails.Status = StatusCodes.Status400BadRequest;
                        }
                        else
                        {
                            problemDetails.Title = "An unexpected error occurred!";
                            problemDetails.Status = StatusCodes.Status500InternalServerError;
                        }

                        context.Response.ContentType = "application/problem+json";
                        context.Response.StatusCode = problemDetails.Status.Value;

                        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
                    });
                });
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.Select().Expand().Filter().OrderBy().Count().MaxTop(10);
                endpoints.MapODataRoute("odata", "odata", GetEdmModel());
            });            
        }
    }
}
