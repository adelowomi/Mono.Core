using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.Prove
{
    public class ProveService : IMonoProve
    {
        private readonly IProveService _proveService;

        public ProveService(IRefitClientBuilder<IProveService> proveService)
        {
            // Prove still lives under /v1/.
            _proveService = proveService.BuildV1(ServiceTypes.Prove);
        }

        public async Task<MonoStandardResponse<InitiateProveResponse>> InitiateProve(
            InitiateProveModel model,
            CancellationToken cancellationToken = default)
        {
            var response = await _proveService.InitiateProve(model, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<ProveCustomerResponse>> FetchCustomerDetails(
            string reference,
            CancellationToken cancellationToken = default)
        {
            var response = await _proveService.FetchCustomerDetails(reference, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<ProveCustomerListResponse>> FetchAllCustomerDetails(
            ProveCustomerListQueryOptions options = null,
            CancellationToken cancellationToken = default)
        {
            var response = await _proveService.FetchAllCustomerDetails(options ?? new ProveCustomerListQueryOptions(), cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<ProveCustomerResponse>> BlacklistCustomer(
            BlacklistCustomerModel model,
            CancellationToken cancellationToken = default)
        {
            var response = await _proveService.BlacklistCustomer(model, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<ProveCustomerResponse>> WhitelistCustomer(
            WhitelistCustomerModel model,
            CancellationToken cancellationToken = default)
        {
            var response = await _proveService.WhitelistCustomer(model, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<dynamic>> RevokeDataAccess(
            string reference,
            CancellationToken cancellationToken = default)
        {
            var response = await _proveService.RevokeDataAccess(reference, cancellationToken);
            return response.HandleResponse();
        }
    }
}
