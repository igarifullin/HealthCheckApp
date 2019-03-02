# HealthCheckApp
Source of application, which start background job and check healthchecks to write into dictionary. It will be useful to get status from cache, because it will be faster than original ms health check

What to look for:
* [TimedHostedService](https://github.com/igarifullin/HealthCheckApp/blob/master/HostedServices/TimedHostedService.cs) - it is background job, which triggers IHealthCheckMonitor
* [HealthCheckMonitor](https://github.com/igarifullin/HealthCheckApp/blob/master/Services/HealthCheckMonitor.cs) - this service calls HealthCheckService and save health reports to storage
* [MemoryHealthCheckStorage](https://github.com/igarifullin/HealthCheckApp/blob/master/Storages/MemoryHealthCheckStorage.cs) - report's storage based on Dictionary
* [HealthCheckMiddleware](https://github.com/igarifullin/HealthCheckApp/blob/master/Middleware/HealthCheckMiddleware.cs) - it is middleware, which provides access to cache reports. It doesn't spent time to start and wait for all health checks 
