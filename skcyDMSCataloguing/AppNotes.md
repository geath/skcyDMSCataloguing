
## Modelling


## Sorting and Filtering 



## Logging
### Implement Logging
1. Add Logging Provider
-install nuget package NLog.Web.AspNetCore abd create nlog.config
-update Program.cs by adding at ```Host.CreateDefaultBuilder(args)``` 
the logging providers by calling ```.ConfigureLogging``` extension method
--for development only EnableSensitiveDataLogging
 

## Further Reading - Sources
- [Nlog ReadMe](https://github.com/NLog/NLog/wiki/Configuration-file#configuration-file-format)



## Model Validation
### Remote Validation
- Create a method that encapsulates and performs the validation logic
- Decorate the attribute with GET and POST Verbs. The GET is required because as the user 
  enters the value, the client side script issues a GET request  to the server
- return JSON true/false based on the evaluation of the validation's logic. (jquery validate method is fired
  to perform the validation and for this method JSON result is expected )
  
  - Server=WEBSRV1\\SAFETICA;Database=skcyDMSCataloguing;Trusted_Connection=True;MultipleActiveResultSets=true