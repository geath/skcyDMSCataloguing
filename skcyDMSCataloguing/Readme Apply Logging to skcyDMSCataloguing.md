#Logging

## Implement Logging
1. Add Logging Provider
-install nuget package NLog.Web.AspNetCore abd create nlog.config
-update Program.cs by adding at ```Host.CreateDefaultBuilder(args)``` 
the logging providers by calling ```.ConfigureLogging``` extension method
--for development only EnableSensitiveDataLogging
 

## Further Reading - Sources
- [Nlog ReadMe](https://github.com/NLog/NLog/wiki/Configuration-file#configuration-file-format)



