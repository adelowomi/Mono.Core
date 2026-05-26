using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.Watchlist
{
    public class WatchlistService : IMonoWatchlist
    {
        private readonly IWatchlistService _watchlistService;

        public WatchlistService(IRefitClientBuilder<IWatchlistService> watchlistService)
        {
            // Watchlist Screening is v3-only.
            _watchlistService = watchlistService.BuildV3(ServiceTypes.Lookup);
        }

        public async Task<MonoStandardResponse<ScreeningResponse>> SubmitIndividualScreening(
            SubmitIndividualScreeningModel model,
            CancellationToken cancellationToken = default)
        {
            var response = await _watchlistService.SubmitIndividualScreening(model, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<ScreeningResponse>> SubmitEntityScreening(
            SubmitEntityScreeningModel model,
            CancellationToken cancellationToken = default)
        {
            var response = await _watchlistService.SubmitEntityScreening(model, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<BatchScreeningResponse>> SubmitBatchScreening(
            BatchScreeningModel model,
            CancellationToken cancellationToken = default)
        {
            var response = await _watchlistService.SubmitBatchScreening(model, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<ScreeningResponse>> GetScreeningResult(
            string id,
            CancellationToken cancellationToken = default)
        {
            var response = await _watchlistService.GetScreeningResult(id, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<AuditLogResponse>> GetAuditLog(
            string id,
            CancellationToken cancellationToken = default)
        {
            var response = await _watchlistService.GetAuditLog(id, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<byte[]>> GetScreeningReport(
            string id,
            CancellationToken cancellationToken = default)
        {
            using (var response = await _watchlistService.GetScreeningReport(id, cancellationToken))
            {
                if (response.IsSuccessStatusCode)
                {
                    var bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                    var ok = MonoStandardResponse<byte[]>.Ok(bytes);
                    ok.Status = ((int)response.StatusCode).ToString();
                    return ok;
                }

                var errorJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                try
                {
                    var parsed = JsonSerializer.Deserialize<MonoStandardResponse<byte[]>>(errorJson);
                    if (parsed != null)
                    {
                        parsed.Success = false;
                        return parsed;
                    }
                }
                catch (JsonException)
                {
                    // Fall through to a generic error wrapper.
                }

                return MonoStandardResponse<byte[]>.Error(
                    string.IsNullOrEmpty(errorJson)
                        ? $"PDF report request failed with status {(int)response.StatusCode}"
                        : errorJson);
            }
        }

        public async Task<MonoStandardResponse<MonitoringResponse>> StartMonitoring(
            StartMonitoringModel model,
            CancellationToken cancellationToken = default)
        {
            var response = await _watchlistService.StartMonitoring(model, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<dynamic>> StopMonitoring(
            string id,
            CancellationToken cancellationToken = default)
        {
            var response = await _watchlistService.StopMonitoring(id, cancellationToken);
            return response.HandleResponse();
        }
    }
}
