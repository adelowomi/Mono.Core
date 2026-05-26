using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace Mono.Core.Watchlist
{
    // Refit interface for the Mono Watchlist Screening API (v3).
    // Uses BuildV3() so the base URL is rewritten from /v2/ to /v3/ before
    // every call; paths here omit the version prefix.
    public interface IWatchlistService
    {
        // -------- Submit / batch screening --------

        [Post("/lookup/watchlist")]
        Task<IApiResponse<MonoStandardResponse<ScreeningResponse>>> SubmitIndividualScreening(
            [Body] SubmitIndividualScreeningModel model,
            CancellationToken cancellationToken = default);

        [Post("/lookup/watchlist")]
        Task<IApiResponse<MonoStandardResponse<ScreeningResponse>>> SubmitEntityScreening(
            [Body] SubmitEntityScreeningModel model,
            CancellationToken cancellationToken = default);

        [Post("/lookup/watchlist/batch")]
        Task<IApiResponse<MonoStandardResponse<BatchScreeningResponse>>> SubmitBatchScreening(
            [Body] BatchScreeningModel model,
            CancellationToken cancellationToken = default);

        // -------- Fetch screening data --------

        [Get("/lookup/watchlist/{id}")]
        Task<IApiResponse<MonoStandardResponse<ScreeningResponse>>> GetScreeningResult(
            string id,
            CancellationToken cancellationToken = default);

        [Get("/lookup/watchlist/{id}/audit-log")]
        Task<IApiResponse<MonoStandardResponse<AuditLogResponse>>> GetAuditLog(
            string id,
            CancellationToken cancellationToken = default);

        // PDF binary download — bypasses MonoStandardResponse<T> wrapping.
        // The high-level facade reads the bytes (or parses the error JSON
        // on a non-2xx response) and wraps the result.
        [Get("/lookup/watchlist/{id}/report")]
        Task<HttpResponseMessage> GetScreeningReport(
            string id,
            CancellationToken cancellationToken = default);

        // -------- Ongoing monitoring --------

        [Post("/lookup/watchlist/monitor")]
        Task<IApiResponse<MonoStandardResponse<MonitoringResponse>>> StartMonitoring(
            [Body] StartMonitoringModel model,
            CancellationToken cancellationToken = default);

        [Delete("/lookup/watchlist/monitor/{id}")]
        Task<IApiResponse<MonoStandardResponse<dynamic>>> StopMonitoring(
            string id,
            CancellationToken cancellationToken = default);
    }
}
