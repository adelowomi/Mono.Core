using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.Miscellaneous
{
    public class MiscellaneousService : IMonoMiscellaneous
    {
        private readonly IMiscellaneousService _miscellaneousService;

        public MiscellaneousService(IRefitClientBuilder<IMiscellaneousService> miscellaneousService)
        {
            _miscellaneousService = miscellaneousService.Build();
        }

          /// <summary>
        /// Retrieves coverage information from the miscellaneous service.
        /// </summary>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with an InstitutionResponseModel.</returns>
        public async Task<MonoStandardResponse<InstitutionResponseModel>> GetCoverage(CancellationToken cancellationToken = default)
        {
          var response = await _miscellaneousService.GetCoverage(cancellationToken);
             return response.HandleResponse();
        }


          /// <summary>
        /// Retrieves CAC lookup information from the miscellaneous service.
        /// </summary>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with a BusinessLookUpResponseModel.</returns>
        public async Task<MonoStandardResponse<BusinessLookUpResponseModel>> GetCacLookup(CancellationToken cancellationToken = default)
        {
           var response = await _miscellaneousService.GetCacLookup(cancellationToken);
             return response.HandleResponse();
        }


          /// <summary>
        /// Retrieves CAC company information for a specified business ID.
        /// </summary>
        /// <param name="businessId">The ID of the business for which to retrieve CAC company information.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with a ShareHolderResponseModel.</returns>
        public async Task<MonoStandardResponse<ShareHolderResponseModel>> GetCacCompany(string businessId, CancellationToken cancellationToken = default)
        {
          var response = await _miscellaneousService.GetCacCompany(businessId, cancellationToken);
             return response.HandleResponse();
        }


         /// <summary>
        /// Unlinks a specified account based on the provided account ID.
        /// </summary>
        /// <param name="accountId">The ID of the account to be unlinked.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with a string indicating the result of the unlink operation.</returns>
        public async Task<MonoStandardResponse<string>> UnlinkAccount(string accountId, CancellationToken cancellationToken = default)
        {
          var response = await _miscellaneousService.UnlinkAccount(accountId, cancellationToken);
             return response.HandleResponse();
        }

    }
}
