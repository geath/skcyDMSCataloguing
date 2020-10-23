
# A. Modify Authentication
The bellow presented workflow implements Identity from scratch without using the scafolder which is recommended because of its simplicity and efficiency. 
[Reading Source Microsoft Docs : Scaffold Identity in ASP.NET Core projects](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-3.1&tabs=visual-studio)

1. Install Nuget packages : Microsoft.AspNetCore.Identity, Microsoft.AspNetCore.Identity.UI , Microsoft.AspNetCore.Identity.EntityFramework, Microsoft.EntityFrameworkCore.SqlServer (if the persistent store is Microsoft SQL Server)

2. Change the Application's DBContext to inherit from ```IdentityDbContext<IdentityUser>```

3. Modify OnModOnModelCreatingel method of DBContext class by adding  ```base.OnModelCreating(modelBuilder);```. 
   OnModelCreating is where IdentityDbContext<ApplicationUser> configures the entity framework mappings so
   when overriding the base class must be called  in order the mapping to be auto configure      

4. Add the login partial (_LoginPartial) to the Views/Shared/_Layout.cshtml file

5. Add code in <span style="color:blue">**ConfigureServicess** method</span> to set the sql database provider as the store for authentication's tables and data
            ```services.AddIdentity<IdentityUser, IdentityRole>()
                      .AddEntityFrameworkStores<AppDbContext>();```
    ```
        
5. Install Nuget package and run migrations 
    ```Install-Package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
      Add-Migration MigrationsName
      Update-Database 
```
    
6. Update the Startup class 
   - in <span style="color:blue">**ConfigureServicess** method</span> add 
        ```services.AddControllersWithViews();
           services.AddRazorPages(); 
        ```
        
   - in <span style="color:blue">**Configure** method</span> add  to the app.UseEndpoints
        ```endpoints.MapRazorPages();```
   
   

## A.I Add Custom User Data to identity



# B. Enable Authorization



## B1. Customize Startup class
1. Confirm that Razor Pages services are added in <span style="color:blue">**ConfigureServicess** method</span> by calling
        ```services.AddRazorPages();```
2. If using the TokenProvider, register the service.    
3. Set Authentication and Authorization middleware in <span style="color:blue">**Configure** method </span> , right after Routing middleware
         ```app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();```
4. Set an endpoint for Razor Pages in <span style="color:blue">**Configure** method </span> 
      ```app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });     
```
      
      [Source: Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-3.1&tabs=visual-studio#scaffold-identity-into-an-mvc-project-without-existing-authorization)

### Suggested  Techniques for Authorization 

- **Require all users to be authenticated.** Modify <span style="color:blue">**ConfigureServicess** method</span> of <span style="color:blue">**Startup** class</span> as follows:
           
               ``` services.AddAuthorization(options =>
                    {
                        options.FallbackPolicy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
            }); ```
   [Source: Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/secure-data?view=aspnetcore-3.1#rau)
   an alternative using ```services.Configure<IdentityOptions>(options=>..)``` is presented in
   [ASP NET core identity password complexity 4:40](https://www.youtube.com/watch?v=kC9qrUcy2Js)
       ```



## B2. Create Controller and Views

1. Create appropriate ViewModels (e.g. RegisterViewModel) and build the AccountController 
2. Modify(optional) the password complexity either by calling ```services.Configure<IdentityOptions>(options=>..)``` in Startup class
   or by calling ```services.AddIdentity<IdentityUser, IdentityRole>(options=>..)``` in Startup class    
3. Inject to _LayoutView SignInManager & UserManager classes in order to encapsulate login/logout logic 
4. Decorate login and register methods with ```[AllowAnonymous]``` attribute to  to avoid falling in an infinite loop















[Recommended Tutorial by PluralSight : Authentication and Authorization in ASP.NET Core ] (https://www.pluralsight.com/courses/authentication-authorization-aspnet-core)


