using System;
using System.Threading;
using System.Threading.Tasks;
using HealthCheckApp.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HealthCheckApp.HostedServices
{
    /// <summary>
    /// Служба запускаема в фоне
    /// </summary>
    internal class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IHealthCheckMonitor _healthCheckMonitor;
        private Timer _timer;

        public TimedHostedService(ILogger<TimedHostedService> logger,
            IHealthCheckMonitor healthCheckMonitor)
        {
            _logger = logger;
            _healthCheckMonitor = healthCheckMonitor;
        }

        /// <inheritdoc cref="IHostedService"/>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, cancellationToken, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Timed Background Service is working.");

            var cancellationToken = (CancellationToken)state;

            _healthCheckMonitor.CheckAsync(cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <inheritdoc cref="IHostedService"/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        /// <inheritdoc cref="IDisposable"/>
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}