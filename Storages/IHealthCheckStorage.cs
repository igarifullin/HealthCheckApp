using System.Collections.Generic;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheckApp.Storages
{
    /// <summary>
    /// Provides an access to health check storage 
    /// </summary>
    public interface IHealthCheckStorage
    {
        /// <summary>
        /// Writes application's status to storage
        /// </summary>
        /// <param key="name">Application's name</param>
        /// <param key="result">Check report</param>
        void Write(string name, HealthReportEntry result);

        /// <summary>
        /// Get all applications check reports
        /// </summary>
        /// <returns>An instance of <see cref="IDictionary<string, HealthCheckResult>"/>.</returns>
        IDictionary<string, HealthReportEntry> GetStatuses();
    }
}