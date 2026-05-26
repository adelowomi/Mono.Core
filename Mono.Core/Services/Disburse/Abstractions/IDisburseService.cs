using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace Mono.Core.Disburse
{
    // Refit interface for the Mono Disburse API (v3). The high-level
    // IMonoDisburse facade wraps these calls and unwraps the response.
    // BaseUrl carries /v2/; the RefitClientBuilder.BuildV3() override
    // rewrites it to /v3/ before requests go out, so paths here omit
    // the version prefix.
    public interface IDisburseService
    {
        // -------- Source accounts --------

        [Post("/payments/disburse/source-accounts")]
        Task<IApiResponse<MonoStandardResponse<SourceAccountResponse>>> CreateSourceAccount(
            [Body] CreateSourceAccountModel model,
            CancellationToken cancellationToken = default);

        [Put("/payments/disburse/source-accounts")]
        Task<IApiResponse<MonoStandardResponse<SourceAccountResponse>>> UpdateSourceAccount(
            [Body] UpdateSourceAccountModel model,
            CancellationToken cancellationToken = default);

        [Get("/payments/disburse/source-accounts")]
        Task<IApiResponse<MonoStandardResponse<SourceAccountListResponse>>> FetchAllSourceAccounts(
            [Query] SourceAccountListQueryOptions options,
            CancellationToken cancellationToken = default);

        [Get("/payments/disburse/source-accounts/{id}")]
        Task<IApiResponse<MonoStandardResponse<SourceAccountResponse>>> FetchSourceAccount(
            string id,
            CancellationToken cancellationToken = default);

        // -------- Disbursements (batches) --------

        [Post("/payments/disburse/disbursements")]
        Task<IApiResponse<MonoStandardResponse<DisbursementResponse>>> CreateDisbursement(
            [Body] CreateDisbursementModel model,
            CancellationToken cancellationToken = default);

        [Post("/payments/disburse/disbursements/{id}/transition")]
        Task<IApiResponse<MonoStandardResponse<DisbursementResponse>>> TransitionDisbursement(
            string id,
            [Body] TransitionDisbursementModel model,
            CancellationToken cancellationToken = default);

        [Get("/payments/disburse/disbursements")]
        Task<IApiResponse<MonoStandardResponse<DisbursementListResponse>>> FetchAllDisbursements(
            [Query] DisbursementListQueryOptions options,
            CancellationToken cancellationToken = default);

        [Get("/payments/disburse/disbursements/{id}")]
        Task<IApiResponse<MonoStandardResponse<DisbursementResponse>>> FetchDisbursement(
            string id,
            CancellationToken cancellationToken = default);

        // -------- Distributions inside a batch --------

        [Post("/payments/disburse/disbursements/{id}/distributions")]
        Task<IApiResponse<MonoStandardResponse<DistributionListResponse>>> AddDistributionsToBatch(
            string id,
            [Body] AddDistributionsModel model,
            CancellationToken cancellationToken = default);

        [Patch("/payments/disburse/disbursements/{id}/distributions/{distributionId}")]
        Task<IApiResponse<MonoStandardResponse<DistributionResponse>>> UpdateDistributionInBatch(
            string id,
            string distributionId,
            [Body] UpdateDistributionModel model,
            CancellationToken cancellationToken = default);

        [Delete("/payments/disburse/disbursements/{id}/distributions/{distributionId}")]
        Task<IApiResponse<MonoStandardResponse<dynamic>>> DeleteDistributionInBatch(
            string id,
            string distributionId,
            CancellationToken cancellationToken = default);

        [Get("/payments/disburse/disbursements/{id}/distributions")]
        Task<IApiResponse<MonoStandardResponse<DistributionListResponse>>> FetchAllDistributionsInBatch(
            string id,
            [Query] DistributionListQueryOptions options,
            CancellationToken cancellationToken = default);

        [Get("/payments/disburse/disbursements/{id}/distributions/{distributionId}")]
        Task<IApiResponse<MonoStandardResponse<DistributionResponse>>> FetchSingleDistribution(
            string id,
            string distributionId,
            CancellationToken cancellationToken = default);
    }
}
