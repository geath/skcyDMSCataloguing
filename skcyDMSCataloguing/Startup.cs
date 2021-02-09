using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using skcyDMSCataloguing.DAL;
 
using skcyDMSCataloguing.DAL.Repositories;
 using skcyDMSCataloguing.Models;
using skcyDMSCataloguing.Services;

namespace skcyDMSCataloguing
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("skcyDMSCataloguing")));

            services.AddAuthentication(IISDefaults.AuthenticationScheme);

            services.AddIdentity<IdentityUser, IdentityRole>()
             .AddEntityFrameworkStores<AppDbContext>() ;

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = ".skcyDMSCataloguingCookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
                options.SlidingExpiration = true;
            });

            services.AddScoped<IBaseAsyncRepo<Box>, BaseAsyncRepo<Box>>();
            services.AddScoped<IBaseAsyncRepo<Folder>, BaseAsyncRepo<Folder>>();
            services.AddScoped<IBaseAsyncRepo<BoxCreator>, BaseAsyncRepo<BoxCreator>>();
            services.AddScoped<IBaseAsyncRepo<CustData>, BaseAsyncRepo<CustData>>();
            services.AddScoped<IBaseAsyncRepo<CustAccData>, BaseAsyncRepo<CustAccData>>();
            services.AddScoped<IBaseAsyncRepo<CustRelData>, BaseAsyncRepo<CustRelData>>();
            services.AddScoped<IBaseAsyncRepo<PrjHelix1>, BaseAsyncRepo<PrjHelix1>>();
            services.AddScoped<IBaseAsyncRepo<PrjVelocity1>, BaseAsyncRepo<PrjVelocity1>>();
            services.AddScoped<IBaseAsyncRepo<PrjVelocity2>, BaseAsyncRepo<PrjVelocity2>>();
            services.AddScoped<IGetObjectType, GetObjectType>();


            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddHttpContextAccessor();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment() || _env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
             

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

            });
        }
    }
}
