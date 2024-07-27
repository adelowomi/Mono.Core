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

        public async Task<MonoStandardResponse<AuthorizationAccountResponseModel>> AuthorizeAccount(AuthorizationAccountRequestModel authorizationAccountRequestModel, CancellationToken cancellationToken = default)
        {
            var response = await _authorizationService.AuthorizeAccount(authorizationAccountRequestModel, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<ManualDataSyncResponseModel>> SyncAccount(string accountId, CancellationToken cancellationToken = default)
        {
            var response = await _authorizationService.SyncAccount(accountId, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<ReAuthorizationCodeResponseModel>> ReauthorizeAccount(string accountId, CancellationToken cancellationToken = default)
        {
            var response = await _authorizationService.ReauthorizeAccount(accountId, cancellationToken);
            return response.HandleResponse();
        }
    }
}
