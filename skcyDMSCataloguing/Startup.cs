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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                                                                  .EnableSensitiveDataLogging());
            services.AddAuthentication(IISDefaults.AuthenticationScheme);

            services.AddIdentity<IdentityUser, IdentityRole>()
             .AddEntityFrameworkStores<AppDbContext>() ;

            //services.ConfigureApplicationCookie(options =>
            //{                
            //    options.Cookie.Name = ".skcyDMSCataloguingCookie";
            //    options.Cookie.HttpOnly = true;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(1);                
            //    options.SlidingExpiration = true;
            //});

            services.AddScoped<IBaseAsyncRepo<Box>, BaseAsyncRepo<Box>>();
            services.AddScoped<IBaseAsyncRepo<Folder>, BaseAsyncRepo<Folder>>();
            services.AddScoped<IBaseAsyncRepo<BoxCreator>, BaseAsyncRepo<BoxCreator>>();
            services.AddScoped<IBaseAsyncRepo<CustData>, BaseAsyncRepo<CustData>>();
            services.AddScoped<IBaseAsyncRepo<CustAccData>, BaseAsyncRepo<CustAccData>>();
            services.AddScoped<IBaseAsyncRepo<CustRelData>, BaseAsyncRepo<CustRelData>>();
            services.AddScoped<IGetObjectType, GetObjectType>();


            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddHttpContextAccessor();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
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
