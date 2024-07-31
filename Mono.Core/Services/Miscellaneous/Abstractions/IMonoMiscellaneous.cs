using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.Miscellaneous
{
    public interface IMonoMiscellaneous
    {
          /// <summary>
        /// This method retrieves coverage information from the miscellaneous service.
        /// </summary>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with an InstitutionResponseModel.</returns>
        Task<MonoStandardResponse<InstitutionResponseModel>> GetCoverage(CancellationToken cancellationToken = default);

         /// <summary>
        /// This method retrieves CAC lookup information from the miscellaneous service.
        /// </summary>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with a BusinessLookUpResponseModel.</returns>
        Task<MonoStandardResponse<BusinessLookUpResponseModel>> GetCacLookup(CancellationToken cancellationToken = default);

          /// <summary>
        /// This method retrieves CAC company information for a specified business ID.
        /// </summary>
        /// <param name="businessId">The ID of the business for which to retrieve CAC company information.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with a ShareHolderResponseModel.</returns>
        Task<MonoStandardResponse<ShareHolderResponseModel>> GetCacCompany(string businessId, CancellationToken cancellationToken = default);

         /// <summary>
        /// This method Unlinks a specified account based on the provided account ID.
        /// </summary>
        /// <param name="accountId">The ID of the account to be unlinked.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with a string indicating the result of the unlink operation.</returns>
        Task<MonoStandardResponse<string>> UnlinkAccount(string accountId, CancellationToken cancellationToken = default);
    }
}
