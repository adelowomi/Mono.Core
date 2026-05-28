using System.Threading;
using System.Threading.Tasks;
using Mono.Core.Accounts;
using Refit;

namespace Mono.Core.Authorization
{
    // this is the refit interface
    public interface IAuthorizationService
    {
        [Post("/accounts/initiate")]
        Task<IApiResponse<MonoStandardResponse<AccountLinkingResponseModel>>> InitiateAccountLinking([Body] AccountLinkingModel accountLinkingModel, CancellationToken cancellationToken = default);

        // accounts/auth — exchanges a Mono Connect code for a permanent account id.
        // The path is plural; an earlier typo here ('/account/auth') caused every
        // link-exchange to surface as MONO_LINK_EXPIRED because Mono returned 404.
        [Post("/accounts/auth")]
        Task<IApiResponse<MonoStandardResponse<AuthorizationAccountResponseModel>>> AuthorizeAccount([Body] AuthorizationAccountRequestModel authorizationAccountRequestModel, CancellationToken cancellationToken = default);

        // accounts/{accountId}/sync
        [Post("/accounts/{accountId}/sync")]
        Task<IApiResponse<MonoStandardResponse<ManualDataSyncResponseModel>>> SyncAccount(string accountId, CancellationToken cancellationToken = default);

        // accounts/{accountId}/reauthorise
        [Post("/accounts/{accountId}/reauthorise")]
        Task<IApiResponse<MonoStandardResponse<ReAuthorizationCodeResponseModel>>> ReauthorizeAccount(string accountId, CancellationToken cancellationToken = default);
    }
}
