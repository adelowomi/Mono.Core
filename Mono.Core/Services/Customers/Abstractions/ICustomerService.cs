using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace Mono.Core.Customers
{
    // Refit interface for the Mono Customer API (v2). The high-level
    // IMonoCustomers facade wraps these calls and unwraps the response.
    public interface ICustomerService
    {
        // POST /customers — individual variant
        [Post("/customers")]
        Task<IApiResponse<MonoStandardResponse<CustomerResponse>>> CreateIndividualCustomer(
            [Body] CreateIndividualCustomerModel model,
            CancellationToken cancellationToken = default);

        // POST /customers — business variant
        [Post("/customers")]
        Task<IApiResponse<MonoStandardResponse<CustomerResponse>>> CreateBusinessCustomer(
            [Body] CreateBusinessCustomerModel model,
            CancellationToken cancellationToken = default);

        // GET /customers/{id}
        [Get("/customers/{id}")]
        Task<IApiResponse<MonoStandardResponse<CustomerResponse>>> RetrieveCustomer(
            string id,
            CancellationToken cancellationToken = default);

        // GET /customers
        [Get("/customers")]
        Task<IApiResponse<MonoStandardResponse<CustomerListResponse>>> ListCustomers(
            [Query] CustomerListQueryOptions options,
            CancellationToken cancellationToken = default);

        // GET /customers/{id}/transactions
        [Get("/customers/{id}/transactions")]
        Task<IApiResponse<MonoStandardResponse<CustomerTransactionsResponse>>> GetCustomerTransactions(
            string id,
            [Query] CustomerTransactionsQueryOptions options,
            CancellationToken cancellationToken = default);

        // GET /accounts — Mono files "fetch all linked accounts" under the
        // Customer product even though the path is global.
        [Get("/accounts")]
        Task<IApiResponse<MonoStandardResponse<LinkedAccountsResponse>>> FetchAllLinkedAccounts(
            [Query] LinkedAccountsQueryOptions options,
            CancellationToken cancellationToken = default);

        // PATCH /customers/{id}
        [Patch("/customers/{id}")]
        Task<IApiResponse<MonoStandardResponse<CustomerResponse>>> UpdateCustomer(
            string id,
            [Body] UpdateCustomerModel model,
            CancellationToken cancellationToken = default);

        // DELETE /customers/{id}
        [Delete("/customers/{id}")]
        Task<IApiResponse<MonoStandardResponse<dynamic>>> DeleteCustomer(
            string id,
            CancellationToken cancellationToken = default);
    }
}
