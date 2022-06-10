using CustomHandlers.CustomHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace EM.Web
{
    /// <summary>
    /// Startup file
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                // This pushes users to login if not authenticated
                options.CacheProfiles.Add("Default0",
                    new CacheProfile()
                    {
                        Duration = 0,
                        NoStore = true
                    });
            });


            services.AddAuthentication("Cookies")
                 .AddCookie("Cookies", config =>
                 {
                     config.Cookie.Name = "UserLoginCookie"; // Name of cookie     
                     config.LoginPath = "/Auth/Login"; // Path for the redirect to user login page    
                     config.AccessDeniedPath = "/Auth/Error";
                 });

            services.AddAuthorization(config =>
            {
                config.AddPolicy("UserPolicy", policyBuilder =>
                {
                    policyBuilder.UserRequireCustomClaim(ClaimTypes.Email);
                });
            });

            services.AddSession();
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddControllersWithViews().AddNToastNotifyNoty(new NToastNotify.NotyOptions()
            {
                ProgressBar = true,
                Timeout = 5000,
                Theme = "mint"
            });

            services.AddDistributedMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            };

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseNToastNotify();

            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseCookiePolicy(cookiePolicyOptions);

            // who are you?  
            app.UseAuthentication();

            // are you allowed?  
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Auth}/{action=Login}/{id?}");
            });
            AppDomain.CurrentDomain.SetData("ContentRootPath", env.ContentRootPath);
            AppDomain.CurrentDomain.SetData("WebRootPath", env.WebRootPath);
        }
    }   
}
