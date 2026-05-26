using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.Prove
{
    public interface IMonoProve
    {
        /// <summary>
        /// Initiates a Mono Prove KYC verification session. Returns a
        /// <c>mono_url</c> the customer should be redirected to in order to
        /// complete verification, plus a <c>session_id</c> for tracking.
        /// </summary>
        Task<MonoStandardResponse<InitiateProveResponse>> InitiateProve(
            InitiateProveModel model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the verification details for a single Prove customer
        /// (identities, face match, address verification, bank accounts, etc.).
        /// </summary>
        Task<MonoStandardResponse<ProveCustomerResponse>> FetchCustomerDetails(
            string reference,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists every Prove customer with optional pagination and filtering.
        /// </summary>
        Task<MonoStandardResponse<ProveCustomerListResponse>> FetchAllCustomerDetails(
            ProveCustomerListQueryOptions options = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Blacklists a customer. Provide a <see cref="BlacklistCustomerModel.Reason"/>
        /// and a <see cref="BlacklistCustomerModel.Code"/> in the 101-105 range.
        /// </summary>
        Task<MonoStandardResponse<ProveCustomerResponse>> BlacklistCustomer(
            BlacklistCustomerModel model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Reinstates a previously blacklisted customer.
        /// </summary>
        Task<MonoStandardResponse<ProveCustomerResponse>> WhitelistCustomer(
            WhitelistCustomerModel model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Revokes Mono's permission to share this customer's verification data
        /// with your business.
        /// </summary>
        Task<MonoStandardResponse<dynamic>> RevokeDataAccess(
            string reference,
            CancellationToken cancellationToken = default);
    }
}
