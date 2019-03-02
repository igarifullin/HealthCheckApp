using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheckApp
{
    /// <summary>
    /// Fake health check. Returns random result
    /// </summary>
    public class FakeRandomHealthCheck : IHealthCheck
    {
        /// <inheritdoc cref="IHealthCheck" />
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            HealthCheckResult result;
            switch (new Random().Next(2))
            {
                case 0:
                    result = HealthCheckResult.Healthy();
                    break;
                case 1:
                    result = HealthCheckResult.Degraded();
                    break;
                default:
                    result = HealthCheckResult.Unhealthy();
                    break;
            }
            
            return Task.FromResult(result); 
        }
    }
}