using EM.EFContext;
using EM.GenericUnitOfWork.Uow;
using CustomHandlers.CustomHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Claims;
using EM.GenericUnitOfWork.Base;
using EM.Services;

namespace EM.Web
{
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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc(options =>
            {
                // This pushes users to login if not authenticated
                //options.Filters.Add(new AuthorizeFilter());

                options.CacheProfiles.Add("Default0",
                    new CacheProfile()
                    {
                        Duration = 0,
                        NoStore = true
                    });
            });

            //services.AddScoped<IDatabaseContext, DbContext>();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IDatabaseContext, ApplicationDbContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddServices();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

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

            //services.AddScoped<IAuthorizationHandler, PoliciesAuthorizationHandler>();
            //services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();

            services.AddSession();
            services.AddControllers();
            services.AddControllersWithViews();

            services.AddDistributedMemoryCache();

            //services.AddRazorPages().AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dbContext)
        {
            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            };

            dbContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            //app.UseResponseCaching();

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