using System.Threading;
using System.Threading.Tasks;
using Mono.Core.Accounts;

namespace Mono.Core.Authorization
{
    public interface IMonoAuthorization
    {
        Task<MonoStandardResponse<AccountLinkingResponseModel>> InitiateAccountLinking(AccountLinkingModel accountLinkingModel, CancellationToken cancellationToken = default);

        Task<MonoStandardResponse<AuthorizationAccountResponseModel>> AuthorizeAccount(AuthorizationAccountRequestModel authorizationAccountRequestModel, CancellationToken cancellationToken = default);

        Task<MonoStandardResponse<ManualDataSyncResponseModel>> SyncAccount(string accountId, CancellationToken cancellationToken = default);

        Task<MonoStandardResponse<ReAuthorizationCodeResponseModel>> ReauthorizeAccount(string accountId, CancellationToken cancellationToken = default);
    }
}
