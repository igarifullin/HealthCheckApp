using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheckApp.Storages
{
    /// <summary>
    /// Implementation <see cref="IHealthCheckStorage"/> based on dictionary
    /// </summary>
    public class MemoryHealthCheckStorage : IHealthCheckStorage
    {
        private readonly ConcurrentDictionary<string, HealthReportEntry> _dictionary;

        public MemoryHealthCheckStorage()
        {
            _dictionary = new ConcurrentDictionary<string, HealthReportEntry>();
        }

        /// <inheritdoc cref="IHealthCheckStorage" />
        public IDictionary<string, HealthReportEntry> GetStatuses()
        {
            return _dictionary;
        }

        /// <inheritdoc cref="IHealthCheckStorage" />
        public void Write(string name, HealthReportEntry status)
        {
            _dictionary.AddOrUpdate(name, status, (key, oldValue) => status);
        }
    }
}