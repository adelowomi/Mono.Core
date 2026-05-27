using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.Disburse
{
    public class DisburseService : IMonoDisburse
    {
        private readonly IDisburseService _disburseService;

        public DisburseService(IRefitClientBuilder<IDisburseService> disburseService)
        {
            // All Disburse endpoints live under /v3/.
            _disburseService = disburseService.BuildV3(ServiceTypes.Disburse);
        }

        // -------- Source accounts --------

        public async Task<MonoStandardResponse<SourceAccountResponse>> CreateSourceAccount(
            CreateSourceAccountModel model,
            CancellationToken cancellationToken = default)
        {
            var response = await _disburseService.CreateSourceAccount(model, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<SourceAccountResponse>> UpdateSourceAccount(
            UpdateSourceAccountModel model,
            CancellationToken cancellationToken = default)
        {
            var response = await _disburseService.UpdateSourceAccount(model, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<SourceAccountListResponse>> FetchAllSourceAccounts(
            SourceAccountListQueryOptions options = null,
            CancellationToken cancellationToken = default)
        {
            var response = await _disburseService.FetchAllSourceAccounts(options ?? new SourceAccountListQueryOptions(), cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<SourceAccountResponse>> FetchSourceAccount(
            string id,
            CancellationToken cancellationToken = default)
        {
            var response = await _disburseService.FetchSourceAccount(id, cancellationToken);
            return response.HandleResponse();
        }

        // -------- Disbursements (batches) --------

        public async Task<MonoStandardResponse<DisbursementResponse>> CreateDisbursement(
            CreateDisbursementModel model,
            CancellationToken cancellationToken = default)
        {
            var response = await _disburseService.CreateDisbursement(model, cancellationToken);
            return response.HandleResponse();
        }

        public Task<MonoStandardResponse<DisbursementResponse>> CreateInstantDisbursement(
            CreateDisbursementModel model,
            CancellationToken cancellationToken = default)
        {
            model.Type = DisbursementTypeConstants.Instant;
            return CreateDisbursement(model, cancellationToken);
        }

        public Task<MonoStandardResponse<DisbursementResponse>> CreateScheduledDisbursement(
            CreateDisbursementModel model,
            CancellationToken cancellationToken = default)
        {
            model.Type = DisbursementTypeConstants.Scheduled;
            return CreateDisbursement(model, cancellationToken);
        }

        public async Task<MonoStandardResponse<DisbursementResponse>> TransitionDisbursement(
            string id,
            TransitionDisbursementModel model,
            CancellationToken cancellationToken = default)
        {
            var response = await _disburseService.TransitionDisbursement(id, model, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<DisbursementListResponse>> FetchAllDisbursements(
            DisbursementListQueryOptions options = null,
            CancellationToken cancellationToken = default)
        {
            var response = await _disburseService.FetchAllDisbursements(options ?? new DisbursementListQueryOptions(), cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<DisbursementResponse>> FetchDisbursement(
            string id,
            CancellationToken cancellationToken = default)
        {
            var response = await _disburseService.FetchDisbursement(id, cancellationToken);
            return response.HandleResponse();
        }

        // -------- Distributions inside a batch --------

        public async Task<MonoStandardResponse<DistributionListResponse>> AddDistributionsToBatch(
            string disbursementId,
            AddDistributionsModel model,
            CancellationToken cancellationToken = default)
        {
            var response = await _disburseService.AddDistributionsToBatch(disbursementId, model, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<DistributionResponse>> UpdateDistributionInBatch(
            string disbursementId,
            string distributionId,
            UpdateDistributionModel model,
            CancellationToken cancellationToken = default)
        {
            var response = await _disburseService.UpdateDistributionInBatch(disbursementId, distributionId, model, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<dynamic>> DeleteDistributionInBatch(
            string disbursementId,
            string distributionId,
            CancellationToken cancellationToken = default)
        {
            var response = await _disburseService.DeleteDistributionInBatch(disbursementId, distributionId, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<DistributionListResponse>> FetchAllDistributionsInBatch(
            string disbursementId,
            DistributionListQueryOptions options = null,
            CancellationToken cancellationToken = default)
        {
            var response = await _disburseService.FetchAllDistributionsInBatch(disbursementId, options ?? new DistributionListQueryOptions(), cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<DistributionResponse>> FetchSingleDistribution(
            string disbursementId,
            string distributionId,
            CancellationToken cancellationToken = default)
        {
            var response = await _disburseService.FetchSingleDistribution(disbursementId, distributionId, cancellationToken);
            return response.HandleResponse();
        }
    }
}
