using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.Miscellaneous
{
    public interface IMonoMiscellaneous
    {
        Task<MonoStandardResponse<InstitutionResponseModel>> GetCoverage(CancellationToken cancellationToken = default);

        Task<MonoStandardResponse<BusinessLookUpResponseModel>> GetCacLookup(CancellationToken cancellationToken = default);

        Task<MonoStandardResponse<ShareHolderResponseModel>> GetCacCompany(string businessId, CancellationToken cancellationToken = default);

        Task<MonoStandardResponse<string>> UnlinkAccount(string accountId, CancellationToken cancellationToken = default);
    }
}
