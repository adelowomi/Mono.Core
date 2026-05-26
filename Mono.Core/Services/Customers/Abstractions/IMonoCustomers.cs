using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.Customers
{
    public interface IMonoCustomers
    {
        /// <summary>
        /// Creates a unique individual customer. Returns 400 if a customer with the
        /// same identity already exists.
        /// </summary>
        Task<MonoStandardResponse<CustomerResponse>> CreateIndividualCustomer(
            CreateIndividualCustomerModel model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a unique business customer. <c>identity</c>, <c>email</c>,
        /// <c>business_name</c>, <c>address</c>, and <c>phone</c> are all required.
        /// </summary>
        Task<MonoStandardResponse<CustomerResponse>> CreateBusinessCustomer(
            CreateBusinessCustomerModel model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a single customer by id.
        /// </summary>
        Task<MonoStandardResponse<CustomerResponse>> RetrieveCustomer(
            string id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists customers with optional pagination and filters.
        /// </summary>
        Task<MonoStandardResponse<CustomerListResponse>> ListCustomers(
            CustomerListQueryOptions options = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves transactions performed by a customer across Mono's payment products.
        /// Supports filtering by period (e.g. "last12months"), page and linked account.
        /// </summary>
        Task<MonoStandardResponse<CustomerTransactionsResponse>> GetCustomerTransactions(
            string id,
            CustomerTransactionsQueryOptions options = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves every account linked to the business. Mono documents this under
        /// the Customer product even though the path is global. Use the
        /// <c>customer</c> filter on <paramref name="options"/> to scope to a single
        /// customer.
        /// </summary>
        Task<MonoStandardResponse<LinkedAccountsResponse>> FetchAllLinkedAccounts(
            LinkedAccountsQueryOptions options = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates one or more fields on a customer. All fields on the model are optional.
        /// </summary>
        Task<MonoStandardResponse<CustomerResponse>> UpdateCustomer(
            string id,
            UpdateCustomerModel model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a customer.
        /// </summary>
        Task<MonoStandardResponse<dynamic>> DeleteCustomer(
            string id,
            CancellationToken cancellationToken = default);
    }
}
