using System.Threading;
using System.Threading.Tasks;
using Mono.Core.Accounts;

namespace Mono.Core.Authorization
{
    public interface IMonoAuthorization
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountLinkingModel"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<AccountLinkingResponseModel>> InitiateAccountLinking(AccountLinkingModel accountLinkingModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authorizationAccountRequestModel"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<AuthorizationAccountResponseModel>> AuthorizeAccount(AuthorizationAccountRequestModel authorizationAccountRequestModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<ManualDataSyncResponseModel>> SyncAccount(string accountId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<ReAuthorizationCodeResponseModel>> ReauthorizeAccount(string accountId, CancellationToken cancellationToken = default);
    }
}
