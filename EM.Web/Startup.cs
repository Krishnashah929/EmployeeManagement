#region Using
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
#endregion

namespace EM.Web
{
    /// <summary>
    /// Startup file
    /// </summary>
    public class Startup
    {
        #region Ctor
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //For run time compilation
            services.AddRazorPages().AddRazorRuntimeCompilation();

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

            //For cookie authentication
            services.AddAuthentication("Cookies")
                 .AddCookie("Cookies", config =>
                 {
                     config.Cookie.Name = "UserLoginCookie"; // Name of cookie     
                     config.LoginPath = "/Auth/Login"; // Path for the redirect to user login page    
                     config.AccessDeniedPath = "/Auth/Error";
                 });
            //For authorization
            services.AddAuthorization(config =>
            {
                config.AddPolicy("UserPolicy", policyBuilder =>
                {
                    policyBuilder.UserRequireCustomClaim(ClaimTypes.Email);
                });
            });

            services.AddSession();
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            //For toast notifications
            services.AddControllersWithViews().AddNToastNotifyNoty(new NToastNotify.NotyOptions()
            {
                ProgressBar = true,
                Timeout = 5000,
                Theme = "mint"
            });
            //For distributing cache memory
            services.AddDistributedMemoryCache();
        }

        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Cookie policy options
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
            app.UseNToastNotify(); //For Toast notification

            app.UseStaticFiles(); //For static files

            app.UseSession(); //For using session

            app.UseRouting();//For routing

            app.UseCookiePolicy(cookiePolicyOptions);//For cookie policy

            app.UseAuthentication();// who are you?  

            app.UseAuthorization();// are you allowed?  

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
