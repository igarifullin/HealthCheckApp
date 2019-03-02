using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HealthCheckApp.Storages;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace HealthCheckApp.Middleware
{
    public class HealthCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HealthCheckOptions _healthCheckOptions;
        private readonly IHealthCheckStorage _storage;
        private readonly string _path;

        public HealthCheckMiddleware(
            RequestDelegate next,
            IOptions<HealthCheckOptions> healthCheckOptions,
            IHealthCheckStorage storage,
            string path)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (healthCheckOptions == null)
            {
                throw new ArgumentNullException(nameof(healthCheckOptions));
            }

            _next = next;
            _healthCheckOptions = healthCheckOptions.Value;
            _storage = storage;
            _path = path;
        }

        /// <summary>
        /// Processes a request.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }
            
            if (!httpContext.Request.Path.Equals(_path, StringComparison.InvariantCultureIgnoreCase))
            {
                await _next(httpContext);
                return;
            }

            // Get results
            var reports = _storage.GetStatuses();

            httpContext.Response.StatusCode = (int)GetHttpStatusCode(reports.Select(x => x.Value.Status));
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(
                new 
                {
                    reports = reports.Select(x =>
                    new
                    {
                        x.Key,
                        x.Value.Description,
                        x.Value.Exception,
                        Status = Enum.GetName(typeof(HealthStatus), x.Value.Status)
                    })
                }
            ));

            if (!_healthCheckOptions.AllowCachingResponses)
            {
                // Similar to: https://github.com/aspnet/Security/blob/7b6c9cf0eeb149f2142dedd55a17430e7831ea99/src/Microsoft.AspNetCore.Authentication.Cookies/CookieAuthenticationHandler.cs#L377-L379
                var headers = httpContext.Response.Headers;
                headers[HeaderNames.CacheControl] = "no-store, no-cache";
                headers[HeaderNames.Pragma] = "no-cache";
                headers[HeaderNames.Expires] = "Thu, 01 Jan 1970 00:00:00 GMT";
            }
        }

        private HttpStatusCode GetHttpStatusCode(IEnumerable<HealthStatus> reports)
        {
            if (reports.Any(x => x != HealthStatus.Healthy))
            {
                return HttpStatusCode.InternalServerError;
            }
            
            return HttpStatusCode.OK;
        }
    }
}