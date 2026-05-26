using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.Customers
{
    public class CustomerService : IMonoCustomers
    {
        private readonly ICustomerService _customerService;

        public CustomerService(IRefitClientBuilder<ICustomerService> customerService)
        {
            _customerService = customerService.Build(ServiceTypes.Connect);
        }

        public async Task<MonoStandardResponse<CustomerResponse>> CreateIndividualCustomer(
            CreateIndividualCustomerModel model,
            CancellationToken cancellationToken = default)
        {
            var response = await _customerService.CreateIndividualCustomer(model, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<CustomerResponse>> CreateBusinessCustomer(
            CreateBusinessCustomerModel model,
            CancellationToken cancellationToken = default)
        {
            var response = await _customerService.CreateBusinessCustomer(model, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<CustomerResponse>> RetrieveCustomer(
            string id,
            CancellationToken cancellationToken = default)
        {
            var response = await _customerService.RetrieveCustomer(id, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<CustomerListResponse>> ListCustomers(
            CustomerListQueryOptions options = null,
            CancellationToken cancellationToken = default)
        {
            var response = await _customerService.ListCustomers(options ?? new CustomerListQueryOptions(), cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<CustomerTransactionsResponse>> GetCustomerTransactions(
            string id,
            CustomerTransactionsQueryOptions options = null,
            CancellationToken cancellationToken = default)
        {
            var response = await _customerService.GetCustomerTransactions(id, options ?? new CustomerTransactionsQueryOptions(), cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<LinkedAccountsResponse>> FetchAllLinkedAccounts(
            LinkedAccountsQueryOptions options = null,
            CancellationToken cancellationToken = default)
        {
            var response = await _customerService.FetchAllLinkedAccounts(options ?? new LinkedAccountsQueryOptions(), cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<CustomerResponse>> UpdateCustomer(
            string id,
            UpdateCustomerModel model,
            CancellationToken cancellationToken = default)
        {
            var response = await _customerService.UpdateCustomer(id, model, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<dynamic>> DeleteCustomer(
            string id,
            CancellationToken cancellationToken = default)
        {
            var response = await _customerService.DeleteCustomer(id, cancellationToken);
            return response.HandleResponse();
        }
    }
}
