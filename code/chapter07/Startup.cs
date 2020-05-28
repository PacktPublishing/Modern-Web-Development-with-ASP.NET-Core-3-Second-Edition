using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace chapter07
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
            //for Windows authentication
            services.AddAuthentication(IISDefaults.AuthenticationScheme);

            services.AddHttpContextAccessor();

            services.AddSingleton<IAuthorizationHandler, DayOfWeekAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, LocalIpHandler>();

            services
                .AddMvc(options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                );
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = (context) => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.HttpOnly = HttpOnlyPolicy.Always;
                options.Secure = CookieSecurePolicy.SameAsRequest;
            });

            services                
                .AddAuthorization(options =>
                {
                    options.AddPolicy("LocalOnly", builder =>
                    {
                        builder.RequireAssertion(ctx =>
                        {
                            var success = false;
                            if (ctx.Resource is AuthorizationFilterContext mvcContext)
                            {
                                success = IPAddress.IsLoopback(mvcContext.HttpContext.Connection.RemoteIpAddress);
                            }
                            return success;
                        });
                    });

                    options.AddPolicy("Complex", builder =>
                    {
                        //a specific username
                        builder.RequireUserName("admin");
                        //being authenticated
                        builder.RequireAuthenticatedUser();
                        //a claim (Claim) with any one of three options (A, B or C)
                        builder.RequireClaim("Claim", "A", "B", "C");
                        //any of of two roles
                        builder.RequireRole("Admin", "Supervisor");
                    });

                    options.AddPolicy(DayOfWeekRequirement.Name, builder =>
                    {
                        builder.AddRequirements(new DayOfWeekRequirement(DayOfWeek.Friday));
                    });
                })
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/Forbidden";
                    options.LogoutPath = "/Account/Logout";
                    options.ReturnUrlParameter = "ReturnUrl";
                });

            services
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

            //Identity
            /*
            services
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")))
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;

                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredLength = 1;
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            */

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.SlidingExpiration = true;

                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/Forbidden";
                options.LogoutPath = "/Account/Logout";
                options.ReturnUrlParameter = "ReturnUrl";
            });

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {               
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseCors();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseProtectedPaths(new ProtectedPathOptions { Path = "/A/Path", PolicyName = "APolicy" });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                
            });
        }
    }
}
