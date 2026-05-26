using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.Watchlist
{
    public interface IMonoWatchlist
    {
        /// <summary>
        /// Submits an individual for watchlist screening (sanctions, PEP, adverse media).
        /// </summary>
        Task<MonoStandardResponse<ScreeningResponse>> SubmitIndividualScreening(
            SubmitIndividualScreeningModel model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Submits a business / entity for watchlist screening.
        /// </summary>
        Task<MonoStandardResponse<ScreeningResponse>> SubmitEntityScreening(
            SubmitEntityScreeningModel model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Submits multiple individuals and/or entities in a single call. Each entry's
        /// <see cref="WatchlistScreeningSubject.Type"/> picks which fields apply.
        /// </summary>
        Task<MonoStandardResponse<BatchScreeningResponse>> SubmitBatchScreening(
            BatchScreeningModel model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a screening's current state (status, matches, risk score, etc.).
        /// </summary>
        Task<MonoStandardResponse<ScreeningResponse>> GetScreeningResult(
            string id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the lifecycle audit log for a screening.
        /// </summary>
        Task<MonoStandardResponse<AuditLogResponse>> GetAuditLog(
            string id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads the screening's PDF compliance report. On success returns the
        /// raw PDF bytes in <see cref="MonoStandardResponse{T}.Data"/>; on failure
        /// the standard JSON error fields are populated.
        /// </summary>
        Task<MonoStandardResponse<byte[]>> GetScreeningReport(
            string id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Enrolls a subject in ongoing monitoring — Mono will continuously re-screen
        /// and emit webhook events when new matches appear.
        /// </summary>
        Task<MonoStandardResponse<MonitoringResponse>> StartMonitoring(
            StartMonitoringModel model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Cancels ongoing monitoring for the given subject id.
        /// </summary>
        Task<MonoStandardResponse<dynamic>> StopMonitoring(
            string id,
            CancellationToken cancellationToken = default);
    }
}
