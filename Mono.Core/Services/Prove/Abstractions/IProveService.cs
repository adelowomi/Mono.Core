using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace Mono.Core.Prove
{
    // Refit interface for the Mono Prove API. Prove still lives under /v1/,
    // so the high-level facade uses RefitClientBuilder.BuildV1(), which
    // rewrites BaseUrl's /v2/ to /v1/ before each call. Paths here omit the
    // version prefix.
    public interface IProveService
    {
        [Post("/prove/initiate")]
        Task<IApiResponse<MonoStandardResponse<InitiateProveResponse>>> InitiateProve(
            [Body] InitiateProveModel model,
            CancellationToken cancellationToken = default);

        [Get("/prove/customers/{reference}")]
        Task<IApiResponse<MonoStandardResponse<ProveCustomerResponse>>> FetchCustomerDetails(
            string reference,
            CancellationToken cancellationToken = default);

        [Get("/prove/customers")]
        Task<IApiResponse<MonoStandardResponse<ProveCustomerListResponse>>> FetchAllCustomerDetails(
            [Query] ProveCustomerListQueryOptions options,
            CancellationToken cancellationToken = default);

        [Post("/prove/customers/blacklist")]
        Task<IApiResponse<MonoStandardResponse<ProveCustomerResponse>>> BlacklistCustomer(
            [Body] BlacklistCustomerModel model,
            CancellationToken cancellationToken = default);

        [Post("/prove/customers/whitelist")]
        Task<IApiResponse<MonoStandardResponse<ProveCustomerResponse>>> WhitelistCustomer(
            [Body] WhitelistCustomerModel model,
            CancellationToken cancellationToken = default);

        [Delete("/prove/customers/{reference}")]
        Task<IApiResponse<MonoStandardResponse<dynamic>>> RevokeDataAccess(
            string reference,
            CancellationToken cancellationToken = default);
    }
}
