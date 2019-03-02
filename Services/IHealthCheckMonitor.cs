using System.Threading;
using System.Threading.Tasks;

namespace HealthCheckApp.Services
{
    /// <summary>
    /// Service responsible for check the health status
    /// </summary>
    public interface IHealthCheckMonitor
    {
         /// <summary>
        /// Checks the health status.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task CheckAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}