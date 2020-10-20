
# A. Modify Authentication
1. Scafold Identity providing the _Layout view and the DBContext class in use
2. Change the DBContext to inherit from IdentityDbContext<IdentityUser>
3. OnModelCreating is where IdentityDbContext<ApplicationUser> configure the entity framework mappings so
   when overriding for the mapping to be auto configure the base class must be called    
   ```base.OnModelCreating(modelBuilder);```
4. Add the login partial (_LoginPartial) to the Views/Shared/_Layout.cshtml file
   [Scaffold Identity into an MVC project without existing authorization] (https://docs.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-3.1&tabs=visual-studio#scaffold-identity-into-an-mvc-project-without-existing-authorization)
5. Install Nuget package and run migrations 
   ```Install-Package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
      Add-Migration MigrationsName
      Update-Database```
6. Update the Startup class 
   - in ConfigureServices method add 
        ```services.AddControllersWithViews();
           services.AddRazorPages(); ```
   - in Configure method add  to the app.UseEndpoints
        ```endpoints.MapRazorPages();```

## A.I Add Custom User Data to identity

# B. Enable Authorization
1. In the Startup class:
    - Confirm that Razor Pages services are added in Startup.ConfigureServices.
    - If using the TokenProvider, register the service.
    - Call UseDatabaseErrorPage on the application builder in Startup.Configure for the Development environment.
    - Call UseAuthentication and UseAuthorization after UseRouting.
    - Add an endpoint for Razor Pages.
    [Enable authentication] (https://docs.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-3.1&tabs=visual-studio#scaffold-identity-into-an-mvc-project-without-existing-authorization)

## Suggested  concepts
1. Require all users to be authenticated. Modify startup class configureservices method as follows
               ``` services.AddAuthorization(options =>
                    {
                        options.FallbackPolicy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
                    }); ```
   [Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/secure-data?view=aspnetcore-3.1#rau)
2. Create Error Controller and corresponding Views
    - use method  Get<IStatusCodeReExecuteFeature>() to access error code numbers
    - use method Get<IExceptionHandlerPathFeature>() to access error paths
3. Create Services\GetObjectType class and Interface. Register them as service in starup class (DI)
##### GetObjectType returns the name of the controller in which the error was raised


# B. Create A&A contrroller and views
1 Create appropriate ViewModels (e.g. RegisterViewModel) and build the AccountController 
2 Modify(optional) the password complexity 
    either by calling ```services.Configure<IdentityOptions>(options=>..)``` in Startup class
    or by calling ```services.AddIdentity<IdentityUser, IdentityRole>(options=>..)``` in Startup class
    [ASP NET core identity password complexity 4:40](https://www.youtube.com/watch?v=kC9qrUcy2Js)
3 inject to _LayoutView SignInManager & UserManager classes in order to encapsulate login/logout logic 









# Implement Logging
1. Employee nlog (external logging provider). Install NLog.Web.AspNetCore nuget package
2. Create nlog.config with the base configuration and the path to local drive where the logs are to be kept. 
    [GitHub Pages](https://github.com/NLog/NLog/wiki/Configuration-file#configuration-file-format).
3. make sure that the "Copy to Output Directory" property of the nlog.config is set to "Copy if newer"
4. Modify Program class adding the logging providers that the app will use
5. In every controller where the logs must be produced inject ILogger<Controller> 
6. Assign warning level to the event that raised an error 
    ``` 
         _logger.LogWarning($"a 404 Error Occured by the user {_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value} . ResourceID = {TempData["ResourceId"]} " +
                                            $" ResourceName : {TempData["ResourceName"]}");
    ```
7. Create (optional) an events' "classification" class (skcyDMSTeamsEvent) to register events with an EventID





# Suggested Learning Resources 
1 Aplicable to A1-3
   - [Entity Mappings using Fluent API in EF 6](https://www.entityframeworktutorial.net/code-first/configure-entity-mappings-using-fluent-api.aspx)
   - [Configuring and Mapping Properties and Types](https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/fluent/types-and-properties)
   - [ASP NET Core Identity tutorial from scratch] (https://www.youtube.com/watch?v=egITMrwMOPU&list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU&index=65)