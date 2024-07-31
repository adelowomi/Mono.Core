using System.Threading;
using System.Threading.Tasks;
using Mono.Core.Accounts;

namespace Mono.Core.Authorization
{
    public class AuthorizationService : IMonoAuthorization
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationService(IRefitClientBuilder<IAuthorizationService> authorizationService)
        {
            _authorizationService = authorizationService.Build();
        }


        public async Task<MonoStandardResponse<AccountLinkingResponseModel>> InitiateAccountLinking(AccountLinkingModel accountLinkingModel, CancellationToken cancellationToken = default)
        {
           var response = await _authorizationService.InitiateAccountLinking(accountLinkingModel, cancellationToken);
              return response.HandleResponse();
        }


          /// <summary>
        /// Authorizes an account based on the provided authorization account request model.
        /// </summary>
        /// <param name="authorizationAccountRequestModel">The model containing the details required for account authorization.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with an AuthorizationAccountResponseModel.</returns>
        public async Task<MonoStandardResponse<AuthorizationAccountResponseModel>> AuthorizeAccount(AuthorizationAccountRequestModel authorizationAccountRequestModel, CancellationToken cancellationToken = default)
        {
            var response = await _authorizationService.AuthorizeAccount(authorizationAccountRequestModel, cancellationToken);
                 return response.HandleResponse();
        }


          /// <summary>
        /// Initiates a manual data synchronization for a specified account ID.
        /// </summary>
        /// <param name="accountId">The ID of the account to synchronize.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with a ManualDataSyncResponseModel.</returns>
        public async Task<MonoStandardResponse<ManualDataSyncResponseModel>> SyncAccount(string accountId, CancellationToken cancellationToken = default)
        {
            var response = await _authorizationService.SyncAccount(accountId, cancellationToken);
             return response.HandleResponse();
        }


          /// <summary>
        /// Reauthorizes a specified account ID.
        /// </summary>
        /// <param name="accountId">The ID of the account to reauthorize.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with a ReAuthorizationCodeResponseModel.</returns>
        public async Task<MonoStandardResponse<ReAuthorizationCodeResponseModel>> ReauthorizeAccount(string accountId, CancellationToken cancellationToken = default)
        {
        var response = await _authorizationService.ReauthorizeAccount(accountId, cancellationToken);
            return response.HandleResponse();
        }

    }
}
