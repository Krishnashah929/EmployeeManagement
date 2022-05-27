using EM.EFContext;
using EM.GenericUnitOfWork.Base;
using EM.GenericUnitOfWork.Uow;
using EM.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using CustomHandlers.CustomHandler;
using System.Threading.Tasks;
using Microsoft.AspNetCore.CookiePolicy;

namespace EM.API
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EM.API", Version = "v1" });
            });
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc(options =>
            {
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dbContext)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EM.API v1"));
            }
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            //app.UseResponseCaching();

            //app.UseCookiePolicy(CookiePolicyOptions);

            app.UseCookiePolicy();

            // who are you?  
            app.UseAuthentication();

            // are you allowed?  
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                    //name: "default",
                    //pattern: "{controller=Auth}/{action=Login}/{id?}");
            });
            AppDomain.CurrentDomain.SetData("ContentRootPath", env.ContentRootPath);
            AppDomain.CurrentDomain.SetData("WebRootPath", env.WebRootPath);
            

        }
    }
}
