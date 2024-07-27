using System.Threading;
using System.Threading.Tasks;
using Mono.Core.Miscellaneous;
using Refit;

namespace Mono.Core
{
    public interface IMiscellaneousService
    {
        // coverage
        [Get("/coverage")]
        Task<IApiResponse<MonoStandardResponse<InstitutionResponseModel>>> GetCoverage(CancellationToken cancellationToken = default);

        // v1/cac/lookup
        [Get("/v1/cac/lookup")]
        Task<IApiResponse<MonoStandardResponse<BusinessLookUpResponseModel>>> GetCacLookup(CancellationToken cancellationToken = default);

        // v1/cac/company/{businessId}
        [Get("/v1/cac/company/{businessId}")]
        Task<IApiResponse<MonoStandardResponse<ShareHolderResponseModel>>> GetCacCompany(string businessId, CancellationToken cancellationToken = default);

        // accounts/{accountId}/unlink
        [Post("/accounts/{accountId}/unlink")]
        Task<IApiResponse<MonoStandardResponse<string>>> UnlinkAccount(string accountId, CancellationToken cancellationToken = default);
    }
}
