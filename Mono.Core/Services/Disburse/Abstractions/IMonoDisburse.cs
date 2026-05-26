using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.Disburse
{
    public interface IMonoDisburse
    {
        // -------- Source accounts --------

        /// <summary>
        /// Registers a bank account that will fund subsequent disbursements.
        /// </summary>
        Task<MonoStandardResponse<SourceAccountResponse>> CreateSourceAccount(
            CreateSourceAccountModel model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a previously registered source account. Only the fields you
        /// set on <paramref name="model"/> are sent.
        /// </summary>
        Task<MonoStandardResponse<SourceAccountResponse>> UpdateSourceAccount(
            UpdateSourceAccountModel model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists every source account registered for this business.
        /// </summary>
        Task<MonoStandardResponse<SourceAccountListResponse>> FetchAllSourceAccounts(
            SourceAccountListQueryOptions options = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Fetches a single source account by id.
        /// </summary>
        Task<MonoStandardResponse<SourceAccountResponse>> FetchSourceAccount(
            string id,
            CancellationToken cancellationToken = default);

        // -------- Disbursements (batches) --------

        /// <summary>
        /// Creates a disbursement batch. Set <see cref="CreateDisbursementModel.Type"/>
        /// to <c>"instant"</c> (executes immediately) or <c>"scheduled"</c> (executes on
        /// <see cref="CreateDisbursementModel.ScheduledDate"/>). Mono uses the same
        /// endpoint for both — only the body differs.
        /// </summary>
        Task<MonoStandardResponse<DisbursementResponse>> CreateDisbursement(
            CreateDisbursementModel model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Convenience wrapper that creates an instant disbursement. Sets
        /// <c>type=instant</c> on <paramref name="model"/>.
        /// </summary>
        Task<MonoStandardResponse<DisbursementResponse>> CreateInstantDisbursement(
            CreateDisbursementModel model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Convenience wrapper that creates a scheduled disbursement. Sets
        /// <c>type=scheduled</c> on <paramref name="model"/>; ensure
        /// <see cref="CreateDisbursementModel.ScheduledDate"/> is populated.
        /// </summary>
        Task<MonoStandardResponse<DisbursementResponse>> CreateScheduledDisbursement(
            CreateDisbursementModel model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Transitions a scheduled disbursement. <c>action=trigger</c> executes it
        /// immediately; <c>action=cancel</c> cancels it.
        /// </summary>
        Task<MonoStandardResponse<DisbursementResponse>> TransitionDisbursement(
            string id,
            TransitionDisbursementModel model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists every disbursement batch for this business.
        /// </summary>
        Task<MonoStandardResponse<DisbursementListResponse>> FetchAllDisbursements(
            DisbursementListQueryOptions options = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Fetches a single disbursement batch by id.
        /// </summary>
        Task<MonoStandardResponse<DisbursementResponse>> FetchDisbursement(
            string id,
            CancellationToken cancellationToken = default);

        // -------- Distributions inside a batch --------

        /// <summary>
        /// Adds one or more distributions to an existing disbursement batch.
        /// </summary>
        Task<MonoStandardResponse<DistributionListResponse>> AddDistributionsToBatch(
            string disbursementId,
            AddDistributionsModel model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a distribution within a batch. All fields on the model are optional.
        /// </summary>
        Task<MonoStandardResponse<DistributionResponse>> UpdateDistributionInBatch(
            string disbursementId,
            string distributionId,
            UpdateDistributionModel model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a distribution from a batch.
        /// </summary>
        Task<MonoStandardResponse<dynamic>> DeleteDistributionInBatch(
            string disbursementId,
            string distributionId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists every distribution attached to a batch.
        /// </summary>
        Task<MonoStandardResponse<DistributionListResponse>> FetchAllDistributionsInBatch(
            string disbursementId,
            DistributionListQueryOptions options = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Fetches a single distribution from a batch by id.
        /// </summary>
        Task<MonoStandardResponse<DistributionResponse>> FetchSingleDistribution(
            string disbursementId,
            string distributionId,
            CancellationToken cancellationToken = default);
    }
}
