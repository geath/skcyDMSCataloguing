using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using skcyDMSCataloguing.DAL;
using skcyDMSCataloguing.DAL.Repositories;
using skcyDMSCataloguing.Models;

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
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>(options=>
                    {
                        options.Password.RequiredLength = 6;
                        options.Password.RequireNonAlphanumeric = true;                        
                    })
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped<IBaseAsyncRepo<Box>, BaseAsyncRepo<Box>>();
            services.AddScoped<IBaseAsyncRepo<Folder>, BaseAsyncRepo<Folder>>();
            services.AddScoped<IBaseAsyncRepo<BoxCreator>, BaseAsyncRepo<BoxCreator>>();
            services.AddScoped<IBaseAsyncRepo<CustData>, BaseAsyncRepo<CustData>>();
            services.AddScoped<IBaseAsyncRepo<CustAccData>, BaseAsyncRepo<CustAccData>>();
            services.AddScoped<IBaseAsyncRepo<CustRelData>, BaseAsyncRepo<CustRelData>>();

            services.AddControllersWithViews();
            services.AddRazorPages();


            services.ConfigureApplicationCookie(options =>
                 {
                     options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                     options.Cookie.Name = "skcyDMSCataloguingCookie";
                     options.LoginPath = "/Account/Login";
                     options.SlidingExpiration = true;
                 }
                ) ;

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.AddPolicy("AdminRolePolicy",
                        policy => policy.RequireRole("Admin"));
                options.AddPolicy("DeleteRolePolicy",
                        policy => policy.RequireClaim("Delete Role" ,"true"));
                
                //  custom authorization policy using func
                options.AddPolicy("EditUserPolicy",
                        policy => policy.RequireAssertion(context =>
                            context.User.IsInRole("Admin") && 
                            context.User.HasClaim(claim=>claim.Type=="Edit User" && claim.Value=="true") ||
                            context.User.IsInRole("PowerUser")
                        ));
                                
                options.AddPolicy("DeleteUserPolicy",
                        policy => policy.RequireClaim("Delete User", "true"));

                options.AddPolicy("CreateAdmEntityPolicy",
                        policy => policy.RequireClaim("Create AdmEntity", "true"));
                options.AddPolicy("ModifyAdmEntityPolicy",
                        policy => policy.RequireClaim("Modify AdmEntity", "true"));
                options.AddPolicy("DeleteAdmEntityPolicy",
                        policy => policy.RequireClaim("Delete AdmEntity", "true"));
                options.AddPolicy("ViewAdmEntityPolicy",
                        policy => policy.RequireClaim("View AdmEntity", "true"));

                options.AddPolicy("CreateBusEntityPolicy",
                        policy => policy.RequireClaim("Create BusEntity", "true"));
                options.AddPolicy("ModifyBusEntityPolicy",
                        policy => policy.RequireClaim("Modify BusEntity", "true"));
                options.AddPolicy("DeleteBusEntityPolicy",
                        policy => policy.RequireClaim("Delete BusEntity", "true"));
                options.AddPolicy("ViewBusEntityPolicy",
                        policy => policy.RequireClaim("View BusEntity", "true"));

            });
            services.AddHttpContextAccessor();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
