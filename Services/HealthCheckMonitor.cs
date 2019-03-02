using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HealthCheckApp.Storages;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheckApp.Services
{
    /// <inheritdoc cref="IHealthCheckMonitor" />
    public class HealthCheckMonitor : IHealthCheckMonitor
    {
        private readonly HealthCheckService _healthCheckService;
        private readonly IHealthCheckStorage _storage;

        public HealthCheckMonitor(HealthCheckService healthCheckService,
            IHealthCheckStorage storage)
        {
            _healthCheckService = healthCheckService;
            _storage = storage;
        }


        /// <inheritdoc cref="IHealthCheckMonitor.CheckAsync" />
        public async Task CheckAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var reports = await _healthCheckService.CheckHealthAsync(cancellationToken);
            foreach (var report in reports.Entries)
            {
                _storage.Write(report.Key, report.Value);
            }
        }
    }
}