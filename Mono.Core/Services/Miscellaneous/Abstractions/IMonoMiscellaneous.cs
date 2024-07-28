using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.Miscellaneous
{
    public interface IMonoMiscellaneous
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<MonoStandardResponse<InstitutionResponseModel>> GetCoverage(CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<MonoStandardResponse<BusinessLookUpResponseModel>> GetCacLookup(CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<MonoStandardResponse<ShareHolderResponseModel>> GetCacCompany(string businessId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<MonoStandardResponse<string>> UnlinkAccount(string accountId, CancellationToken cancellationToken = default);
    }
}
