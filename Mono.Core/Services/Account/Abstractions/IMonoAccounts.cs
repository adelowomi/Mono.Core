using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.Accounts
{
    public interface IMonoAccounts
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
        /// <param name="accountId"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<InformationResponseModel>> GetAccount(string accountId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="jobId"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<StatementPdfResponseModel>> GetPollStatementPdf(string accountId, string jobId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<IncomeResponseModel>> GetIncome(string accountId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<IdentityResponseModel>> GetIdentity(string accountId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="statementRequestModels"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<StatementResponseModel>> GetStatement(string accountId, StatementRequestModels statementRequestModels, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="accountTransactionsOptionsRequest"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<TransactionResponseModel>> GetTransactions(string accountId, AccountTransactionsOptionsRequest accountTransactionsOptionsRequest, CancellationToken cancellationToken = default);
    }
}
