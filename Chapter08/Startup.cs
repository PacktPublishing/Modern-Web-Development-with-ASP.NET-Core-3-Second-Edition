using chapter08.Controllers;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;
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
                .AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
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

            services
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.ReportApiVersions = true;
                });

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

            services
                .AddOData()
                .EnableApiVersioning(options =>
                {
                });

            services.AddODataApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options
                    .QueryOptions
                    .Controller<ProductsController>()
                        .Action(c => c.Get())
                            .Allow(AllowedQueryOptions.Skip | AllowedQueryOptions.Count)
                            .AllowTop(100)
                            .AllowOrderBy();
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ODataSwaggerOptions>();

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<ODataSwaggerOperationFilter>();

                options.IncludeXmlComments(XmlCommentsFilePath());

                options.DocInclusionPredicate((prefix, description) =>
                {
                    return true;
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, VersionedODataModelBuilder modelBuilder, IApiVersionDescriptionProvider provider)
        {
            var models = modelBuilder.GetEdmModels();

            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

                        if (exception is BadHttpRequestException)
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

            app.UseMvc(routes =>
            {
                routes.ServiceProvider.GetRequiredService<ODataOptions>().UrlKeyDelimiter = ODataUrlKeyDelimiter.Parentheses;
                routes.Select().Filter().Expand().OrderBy().Count();
                routes.MapVersionedODataRoutes("odata", "odata/v{version:apiVersion}", models);
                routes.EnableDependencyInjection();
            });

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
            }
        }

        private static string XmlCommentsFilePath()
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            var fileName = $"{typeof(Startup).Assembly.GetName().Name}.xml";
            return Path.Combine(basePath, fileName);
        }
    }
}
